using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    private void Start()
    {
        FindObjectOfType<CombatManager>().isBossAlive = true;
    }

    public override void Death()
    {
        FindObjectOfType<CombatManager>().isBossAlive = false;
        base.Death();   
    }
}
