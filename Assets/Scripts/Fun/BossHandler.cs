using TMPro;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public static BossHandler Instance;
    public bool fighting;
    public Boss activeBoss;
    public GameObject bossBar;
    public SpriteRenderer barFilling;
    public TMP_Text barTitle;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        activeBoss = null;
        fighting = false;
        bossBar.SetActive(false);
        barTitle.text = string.Empty;
        barFilling.color = Color.white;
    }
    public void PrepareBoss(Boss boss)
    {
        activeBoss = boss;
        bossBar.SetActive(true);
        bossBar.GetComponent<Animator>().Play("BossBarUnfold");
        barTitle.text = boss.bossName;
        barFilling.color = boss.bossColor;
    }
    public void StartFight()
    {
        fighting = true;
    }
}
