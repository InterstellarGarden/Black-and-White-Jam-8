using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
    public int maxHealthIncreasePerLoop = 2;
    [SerializeField] private enemyType thisEnemyType;
    protected override void Awake()
    {
        //Increase maxhealth by 2 per loop
        maxHealth += (CarriageManager.loopsCompleted * maxHealthIncreasePerLoop);
        base.Awake();
    }
    protected virtual void Start()
    {
        thisCombatManager.UpdateCountOfEachEnemyType(1, (int)thisEnemyType);
    }
    public override void Death()
    {
        //Tell combat manager to remove active count
        thisCombatManager.UpdateCountOfEachEnemyType(-1, (int)thisEnemyType);
        base.Death();
    }
}
