using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyLegsBehaviour : EnemyBehaviour
{
    [SerializeField]
    private GameObject crate;
    public BoxCollider inBoxCollider, standingCollider;
    bool isStanding = false;
    protected override void Awake()
    {
        base.Awake();
        thisAnimator = GetComponentInChildren<Animator>();
    }
    public override void TriggerTakeDamage(int _bulletType)
    {
        base.TriggerTakeDamage(_bulletType);
    }
    public override void TriggerShootingAnim()
    {
        if (thisAnimator != null && isStanding)
            thisAnimator.Play("standingShoot");

        else base.TriggerShootingAnim();
    }
    public void Exposed()
    {
        isStanding = true;

        inBoxCollider.enabled = false;
        standingCollider.enabled = true;

        thisAnimator.Play("exposed");
        thisAnimator.SetBool("isStanding", true);
    }
    public override void Death()
    {
        //Make crate independent so it is not destroyed.
        crate.transform.SetParent(null);

        if (thisAnimator != null)
        {
            if (isStanding)
                thisAnimator.Play("standingDying");

            else thisAnimator.Play("instantkill");
        }

        //Run Death afterwards.
        base.Death();
    }
}
