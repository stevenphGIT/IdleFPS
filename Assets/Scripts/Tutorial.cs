using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, IDataPersistence
{
    public Vars varRef;
    public Gun gunRef;
    public AvailableUpgrades upgRef;
    public Abilities abilityRef;
    public NoticeBox notice;
    private HitSound hitSound;

    public int tutorialStep;



    // Start is called before the first frame update
    void Start()
    {
        varRef = GameObject.Find("VarTracker").GetComponent<Vars>();
        gunRef = GameObject.Find("GunHandler").GetComponent<Gun>();
        upgRef = GameObject.Find("UpgradeHandler").GetComponent<AvailableUpgrades>();
        abilityRef = GameObject.Find("AbilityHandler").GetComponent<Abilities>();
        hitSound = GameObject.Find("HitAudio").GetComponent<HitSound>();
        notice = GameObject.Find("NoticeHandler").GetComponent<NoticeBox>();

        if (varRef.totalTotalHitCount > 100)
        {
            tutorialStep = -1;
        }
        if (tutorialStep == 0)
        {
            tutorialStep++;
            //hitSound.source.PlayOneShot(hitSound.openMenu, 1f);
            notice.SetTitleColor(Color.white);
            notice.SetTitleText("Welcome!");
            notice.SetDescriptionText("Have you played IdleFPS before?");
            notice.SetChoosable(true);
            notice.idNum = 100;
            notice.ShowBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialStep == -1)
            return;

        if (tutorialStep == 1 && varRef.totalTotalHitCount >= 15)
        {
            tutorialStep++;
            PromptHandgunPurchase();
        }
    }

    private void PromptHandgunPurchase()
    {
        hitSound.source.PlayOneShot(hitSound.openMenu, 1f);
        notice.SetTitleColor(Color.white);
        notice.SetTitleText("Tutorial");
        notice.SetDescriptionText("You've earned 15 Hits! Let's start earning automatically. Press the arrow on the right to navigate to the shop.");
        notice.SetChoosable(false);
        notice.ShowBox();
    }

    public void LoadData(GameData data)
    {
        this.tutorialStep = data.tutorialStep;
    }

    public void SaveData(ref GameData data)
    {
        data.tutorialStep = this.tutorialStep;
    }
}
