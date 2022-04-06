using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyLegsBehaviour : EnemyBehaviour
{
    [SerializeField]
    private GameObject crate;
    public override void Death()
    {
        //Make crate independent so it is not destroyed.
        crate.transform.SetParent(null);

        //Run Death afterwards.
        base.Death();
    }
}
