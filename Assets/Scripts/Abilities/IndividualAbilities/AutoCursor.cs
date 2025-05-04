using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCursor : MonoBehaviour
{
    GameObject target;
    private float waitTime;
    private bool onShield = false;
    private void Start()
    {
        SelectTarget();
    }
    void Update()
    {
        if (target == null)
        {
            SelectTarget();
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, Vars.Instance.autoCursorSpeed * Time.deltaTime);
            if (this.transform.position == target.transform.position)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    if (!onShield)
                    {
                        BoardHandler.Instance.CollectTarget(target.GetComponent<Collider2D>());
                    }
                    else
                        target.GetComponent<SnowShield>().Click();
                }
            }
            else
            {
                if (!onShield)
                    waitTime = 1f / Vars.Instance.autoCursorSpeed;
                else
                    waitTime = 1f;
            }
        }
    }
    void SelectTarget()
    {
        target = GameObject.FindGameObjectWithTag("Shield");
        onShield = true;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Target");
            onShield = false;
        }
    }
}
