using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
    public int maxHealthIncreasePerLoop = 2;
    protected override void Awake()
    {
        //Increase maxhealth by 2 per loop
        maxHealth += (CarriageManager.loopsCompleted * maxHealthIncreasePerLoop);
        base.Awake();
    }
}
