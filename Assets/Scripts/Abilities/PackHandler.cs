using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackHandler : MonoBehaviour
{
    public static PackHandler Instance;

    public Animator orbAnim;

    public Animator cardParentAnim;

    public SpriteRenderer orb1, orb2, orb3;
    public ParticleSystem orbExplosion;
    private ParticleSystem.MainModule orbExplosionMain;

    public AbilityCardObject[] cards = new AbilityCardObject[3];

    public GameObject deckScreen;
    public GameObject packScreen;

    public GameObject cardPrefab;

    public GameObject cardSpawn;

    public GameObject dismissButton;

    public Sprite[] cardFronts;
    public Sprite[] cardBacks;

    private int collectedCards = 0;

    Ability[] commonList;
    Ability[] uncommonList;
    Ability[] rareList;
    Ability[] epicList;
    Ability[] legendaryList;
    Ability[] mythicList;

    List<Ability> packContents = new List<Ability>();

    Color[] rarityColors = {new Color(0.74f, 0.74f, 0.74f), new Color(0f, 0.73f, 0.03f), new Color(0f, 0.38f, 0.81f), new Color(0.67f, 0f, 0.83f), new Color(1f, 0.89f, 0f), new Color(1f, 0f, 0f)};
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        orbExplosionMain = orbExplosion.main;
    }
    void Start()
    {
        commonList = Resources.LoadAll<Ability>("Abilities/0Common/");
        uncommonList = Resources.LoadAll<Ability>("Abilities/1Uncommon/");
        rareList = Resources.LoadAll<Ability>("Abilities/2Rare/");
        epicList = Resources.LoadAll<Ability>("Abilities/3Epic/");
        legendaryList = Resources.LoadAll<Ability>("Abilities/4Legendary/");
        mythicList = Resources.LoadAll<Ability>("Abilities/5Mythic/");
        deckScreen.SetActive(true);
        packScreen.SetActive(false);
    }
    public void OpenPack(int r)
    {
        int tokenRequirement = 0;
        if (r == 0)
            tokenRequirement = 1;
        if (r == 1)
            tokenRequirement = 8;
        if (r == 2)
            tokenRequirement = 50;
        if (Vars.Instance.tokens < tokenRequirement)
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
            FloatingText.Instance.PopText("Not enough <sprite index=2><color=#FF7300>TOKENS</color>!", Color.red, 1);
            return;
        }
        else
        {
            Vars.Instance.tokens -= tokenRequirement;
        }
        for (int i = 0; i < 3; i++)
        {
            int randNum = Random.Range(0, 100);
            //Common Pack Odds
            if (r == 0)
            {
                if (randNum <= 70)
                {
                    packContents.Add(commonList[Random.Range(0, commonList.Length)]);
                }
                else if (randNum <= 90)
                {
                    packContents.Add(uncommonList[Random.Range(0, uncommonList.Length)]);
                }
                else if (randNum <= 98)
                {
                    packContents.Add(rareList[Random.Range(0, rareList.Length)]);
                }
                else
                {
                    packContents.Add(epicList[Random.Range(0, epicList.Length)]);
                }
            }
            //Rare Pack Odds
            if (r == 1)
            {
                if (randNum <= 30)
                {
                    packContents.Add(uncommonList[Random.Range(0, uncommonList.Length)]);
                }
                else if (randNum <= 80)
                {
                    packContents.Add(rareList[Random.Range(0, rareList.Length)]);
                }
                else if (randNum <= 95)
                {
                    packContents.Add(epicList[Random.Range(0, epicList.Length)]);
                }
                else
                {
                    packContents.Add(legendaryList[Random.Range(0, legendaryList.Length)]);
                }
            }
            //Legendary Pack Odds
            if (r == 2)
            {
                if (randNum <= 10)
                {
                    packContents.Add(epicList[Random.Range(0, rareList.Length)]);
                }
                else if (randNum <= 60)
                {
                    packContents.Add(epicList[Random.Range(0, epicList.Length)]);
                }
                else if (randNum <= 95)
                {
                    packContents.Add(legendaryList[Random.Range(0, legendaryList.Length)]);
                }
                else
                {
                    packContents.Add(mythicList[Random.Range(0, mythicList.Length)]);
                }
            }
        }
        //TODO: SUBTRACT TOKENS
        foreach (Ability a in packContents)
        {
            Abilities.Instance.CollectAbility(a);
        }
        
        if (packContents.ElementAt(0).rarity >= packContents.ElementAt(1).rarity && packContents.ElementAt(0).rarity >= packContents.ElementAt(2).rarity)
        {
            orb1.color = rarityColors[packContents.ElementAt(2).rarity];
            orb2.color = rarityColors[packContents.ElementAt(1).rarity];
            orb3.color = rarityColors[packContents.ElementAt(0).rarity];
        }
        else if (packContents.ElementAt(1).rarity >= packContents.ElementAt(0).rarity && packContents.ElementAt(1).rarity >= packContents.ElementAt(2).rarity)
        {
            orb1.color = rarityColors[packContents.ElementAt(0).rarity];
            orb2.color = rarityColors[packContents.ElementAt(2).rarity];
            orb3.color = rarityColors[packContents.ElementAt(1).rarity];
        }
        else 
        {
            orb1.color = rarityColors[packContents.ElementAt(0).rarity];
            orb2.color = rarityColors[packContents.ElementAt(1).rarity];
            orb3.color = rarityColors[packContents.ElementAt(2).rarity];
        }
        StartCoroutine(PlayOpeningAnimation());
    }

    IEnumerator PlayOpeningAnimation()
    {
        PreparePackScreen();
        orbAnim.Play("CardOrbs");
        yield return new WaitForSeconds(7.43f);
        orbExplosionMain.startColor = orb3.color;
        orbExplosion.Play();
        GameObject card1 = Instantiate(cardPrefab, cardSpawn.transform);
        GameObject card2 = Instantiate(cardPrefab, cardSpawn.transform);
        GameObject card3 = Instantiate(cardPrefab, cardSpawn.transform);
        card1.transform.position += new Vector3(-5, 0 ,0);
        card3.transform.position += new Vector3(5, 0, 0);
        cards[0] = card1.GetComponent<AbilityCardObject>();
        cards[1] = card2.GetComponent<AbilityCardObject>();
        cards[2] = card3.GetComponent<AbilityCardObject>();

        cards[0].SetHeldAbility(packContents.ElementAt(0));
        cards[1].SetHeldAbility(packContents.ElementAt(1));
        cards[2].SetHeldAbility(packContents.ElementAt(2));
        cardParentAnim.Play("CardParentZoom");

        yield return new WaitForSeconds(0.8f);
        cards[0].AllowShake();
        cards[1].AllowShake();
        cards[2].AllowShake();
        packContents.Clear();
    }

    public void PreparePackScreen() 
    {
        deckScreen.SetActive(false);
        packScreen.SetActive(true);
    }

    public void CollectCards()
    {
        dismissButton.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            cards[i].animator.Play("CardCollect");
        }
        StartCoroutine(EndPackScreen());
    }

    IEnumerator EndPackScreen()
    {
        yield return new WaitForSeconds(1f);
        deckScreen.SetActive(true);
        packScreen.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            cards[i] = null;
        }
    }

    public void FlipCard()
    {
        collectedCards++;
        if (collectedCards >= 3)
        {
            collectedCards = 0;
            dismissButton.SetActive(true);
        }
    }
}
