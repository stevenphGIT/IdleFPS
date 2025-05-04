using TMPro;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public static BossHandler Instance;
    public bool fighting;
    public Boss activeBoss;
    public GameObject bossBar;
    public SpriteRenderer barFilling;
    public GameObject healthBar, damageBar, targetPos;
    public TMP_Text barTitle;

    public double storedDamage = 0;
    private void Update()
    {
        damageBar.transform.position = Vector3.MoveTowards(damageBar.transform.position, targetPos.transform.position, 0.005f);
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Reset();
    }
    public void Reset()
    {
        activeBoss = null;
        fighting = false;
        bossBar.SetActive(false);
        barTitle.text = string.Empty;
        barFilling.color = Color.white;
        storedDamage = 0;
    }
    public void PrepareBoss(Boss boss)
    {
        activeBoss = boss;
        bossBar.SetActive(true);
        bossBar.GetComponent<Animator>().Play("BossBarUnfold");
        barTitle.text = boss.bossName;
        barFilling.color = boss.bossColor;
        storedDamage = 0;
    }
    public void StartFight()
    {
        fighting = true;
    }

    public void UpdateBossBar()
    {
        float t = (float)activeBoss.health / (float)activeBoss.maxHealth;
        targetPos.transform.localPosition = new Vector3(Mathf.Lerp(-2.34f, 0f, t), targetPos.transform.localPosition.y, targetPos.transform.localPosition.z);
        healthBar.transform.position = targetPos.transform.position;
    }
}
