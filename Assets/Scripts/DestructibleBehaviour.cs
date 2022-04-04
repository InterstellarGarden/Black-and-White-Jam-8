using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehaviour : EntityBehaviour
{
    private Breakable thisBreak;
    private void Awake()
    {
        thisBreak = GetComponentInParent<Breakable>();
    }
    public override void Death()
    {
        thisBreak.Break();
    }
}
