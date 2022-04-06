using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
    protected override void Awake()
    {
        //Increase maxhealth by 2 per loop
        maxHealth += (CarriageManager.loopsCompleted * 2);
        base.Awake();
    }
}
