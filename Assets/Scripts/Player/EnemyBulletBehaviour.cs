using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : BulletBehaviour
{
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterBehaviour>().TriggerTakeDamage(1);
            base.OnCollisionEnter(collision);
        }

        else base.OnCollisionEnter(collision);
    }
}
