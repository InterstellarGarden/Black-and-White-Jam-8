using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
    public int maxHealthIncreasePerLoop = 2;
    [SerializeField] private enemyType thisEnemyType;
    protected Animator thisAnimator;
    protected override void Awake()
    {
        thisAnimator = GetComponent<Animator>();

        //Increase maxhealth by 2 per loop
        maxHealth += (CarriageManager.loopsCompleted * maxHealthIncreasePerLoop);
        base.Awake();
    }
    public override void TriggerTakeDamage(int _bulletType)
    {
        if (thisAnimator != null)
            thisAnimator.Play("shot");
        base.TriggerTakeDamage(_bulletType);
    }
    public virtual void TriggerShootingAnim()
    {
        if (thisAnimator != null)
            thisAnimator.Play("shooting");
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
