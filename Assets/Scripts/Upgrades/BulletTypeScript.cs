using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeScript : MonoBehaviour
{
    private Vars varRef;
    private AvailableUpgrades uRef;
    private Gun gunRef;
    public UpgradeDatabase db;
    private float counter = 0;
    // Update is called once per frame
    void Start()
    {
        varRef = GameObject.Find("VarTracker").GetComponent<Vars>();
        uRef = GameObject.Find("UpgradeHandler").GetComponent<AvailableUpgrades>();
        gunRef = GameObject.Find("GunHandler").GetComponent<Gun>();
        //Auto-Unlocks
        TestForUnlock();
    }

    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 10)
        {
            TestForUnlock();
            counter = 0;
        }
    }

    public void TestForUnlock()
    {
        //Ability Upgrades
        if (varRef.totalHitCount > 90000)
        {
            uRef.Unlock(db.findName(2001));
        }
        if (varRef.totalHitCount > 25)
        {
            uRef.Unlock(db.findName(5555));
        }
        //Target Upgrades
        if (uRef.hasPINBought(9026))
        {
            uRef.Unlock(db.findName(9027));
        }
        if (uRef.hasPINBought(9027))
        {
            uRef.Unlock(db.findName(9028));
        }
        if (uRef.hasPINBought(9028))
        {
            uRef.Unlock(db.findName(9029));
        }
        if (varRef.clickTracker > 100)
        {
            uRef.Unlock(db.findName(9026));
            uRef.Unlock(db.findName(9000));
        }
        if (varRef.clickTracker > 1000 && uRef.hasPINBought(9000))
        {
            uRef.Unlock(db.findName(9001));
        }
        if (varRef.clickTracker > 5000 && uRef.hasPINBought(9001))
        {
            uRef.Unlock(db.findName(9002));
        }
        if (varRef.clickTracker > 10000 && uRef.hasPINBought(9002))
        {
            uRef.Unlock(db.findName(9003));
        }
        if (varRef.silverClickTracker > 10)
        {
            uRef.Unlock(db.findName(9004));
        }
        if (varRef.silverClickTracker > 50 && uRef.hasPINBought(9004))
        {
            uRef.Unlock(db.findName(9005));
        }
        if (varRef.silverClickTracker > 100 && uRef.hasPINBought(9005))
        {
            uRef.Unlock(db.findName(9006));
        }
        if (varRef.silverClickTracker > 200 && uRef.hasPINBought(9006))
        {
            uRef.Unlock(db.findName(9007));
        }
        if (varRef.goldClickTracker > 10)
        {
            uRef.Unlock(db.findName(9008));
        }
        if (varRef.goldClickTracker > 50 && uRef.hasPINBought(9008))
        {
            uRef.Unlock(db.findName(9009));
        }
        if (varRef.goldClickTracker > 100 && uRef.hasPINBought(9009))
        {
            uRef.Unlock(db.findName(9010));
        }
        if (varRef.goldClickTracker > 200 && uRef.hasPINBought(9010))
        {
            uRef.Unlock(db.findName(9011));
        }
        if (varRef.platClickTracker > 10)
        {
            uRef.Unlock(db.findName(9012));
        }
        if (varRef.platClickTracker > 50 && uRef.hasPINBought(9012))
        {
            uRef.Unlock(db.findName(9013));
        }
        if (varRef.platClickTracker > 100 && uRef.hasPINBought(9013))
        {
            uRef.Unlock(db.findName(9014));
        }
        if (varRef.platClickTracker > 200 && uRef.hasPINBought(9014))
        {
            uRef.Unlock(db.findName(9015));
        }
        if (varRef.totalHps > 10 && varRef.clickTracker > 100)
        {
            uRef.Unlock(db.findName(9016));
        }
        if (varRef.totalHps > 100 && varRef.clickTracker > 200 && uRef.hasPINBought(9016))
        {
            uRef.Unlock(db.findName(9017));
        }
        if (varRef.totalHps > 500 && varRef.clickTracker > 300 && uRef.hasPINBought(9017))
        {
            uRef.Unlock(db.findName(9018));
        }
        if (varRef.totalHps > 1000 && varRef.clickTracker > 400 && uRef.hasPINBought(9018))
        {
            uRef.Unlock(db.findName(9019));
        }
        if (varRef.totalHps > 5000 && varRef.clickTracker > 500 && uRef.hasPINBought(9019))
        {
            uRef.Unlock(db.findName(9020));
        }
        if (varRef.totalHps > 10000 && varRef.clickTracker > 600 && uRef.hasPINBought(9020))
        {
            uRef.Unlock(db.findName(9021));
        }
        if (varRef.totalHps > 50000 && varRef.clickTracker > 700 && uRef.hasPINBought(9021))
        {
            uRef.Unlock(db.findName(9022));
        }
        if (varRef.totalHps > 100000 && varRef.clickTracker > 800 && uRef.hasPINBought(9022))
        {
            uRef.Unlock(db.findName(9023));
        }
        if (varRef.totalHps > 500000 && varRef.clickTracker > 900 && uRef.hasPINBought(9023))
        {
            uRef.Unlock(db.findName(9024));
        }
        if (varRef.totalHps > 1000000 && varRef.clickTracker > 1000 && uRef.hasPINBought(9024))
        {
            uRef.Unlock(db.findName(9025));
        }
        //Bullet Types
        if (varRef.totalHitCount > 100)
        {
            uRef.Unlock(db.findName(2000));
        }
        if (varRef.totalHitCount > 50000)
        {
            uRef.Unlock(db.findName(1000));
        }
        if(varRef.totalHitCount > 250000)
        {
            uRef.Unlock(db.findName(1001));
        }
        if(varRef.totalHitCount > 500000)
        {
            uRef.Unlock(db.findName(1002));
        }
        if (varRef.totalHitCount > 2500000)
        {
            uRef.Unlock(db.findName(1003));
        }
        if (varRef.totalHitCount > 5000000)
        {
            uRef.Unlock(db.findName(1004));
            uRef.Unlock(db.findName(1005));
            uRef.Unlock(db.findName(1006));
            uRef.Unlock(db.findName(1007));
            uRef.Unlock(db.findName(1008));
            uRef.Unlock(db.findName(1009));
        }
        if (varRef.totalHitCount > 1000000000000)
        {
            uRef.Unlock(db.findName(9999));
        }
    }
}
