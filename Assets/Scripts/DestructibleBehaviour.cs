using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehaviour : EntityBehaviour
{
    private Breakable thisBreak;
    private EntityBehaviour BabyLegs;
    private void Awake()
    {
        thisBreak = GetComponentInParent<Breakable>();
    }
    public override void Death()
    {
        if (transform.GetComponentInParent<FixedJoint>() != null)
        {
            Destroy(transform.GetComponentInParent<FixedJoint>());
        }

        thisBreak.Break();
    }
}
