using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    protected override void Start()
    {
        FindObjectOfType<CombatManager>().isBossAlive = true;
        base.Start();
    }

    public override void Death()
    {
        FindObjectOfType<CombatManager>().isBossAlive = false;
        FindObjectOfType<CarriageManager>().UpdateIncreaseLoop();
        base.Death();   
    }
}
