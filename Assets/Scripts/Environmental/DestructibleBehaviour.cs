using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehaviour : EntityBehaviour
{
    private Breakable thisBreak;
    private EntityBehaviour BabyLegs;
    protected override void Awake()
    {
        base.Awake();
        thisBreak = GetComponentInParent<Breakable>();
    }
    public override void Death()
    {
        //Baby legs will have Fixedjoint. If this is attached onto baby legs as child, run these lines
        if (transform.GetComponentInParent<FixedJoint>() != null)
        {
            //Seperate crate from babylegs to prevent babylegs from clipping through ground.
            Destroy(transform.GetComponentInParent<FixedJoint>());

            //To Mike: Trigger Babylegs startled from here(?)
            transform.GetComponentInParent<BabyLegsBehaviour>().Exposed();
        }

        thisBreak.Break();
    }
}
