using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : BulletBehaviour
{
    protected override void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DelayDestruction());
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player take damage from trigger");

            other.gameObject.GetComponent<CharacterBehaviour>().TriggerTakeDamage(1);
            Destroy(gameObject);
        }
    }

    IEnumerator DelayDestruction()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
