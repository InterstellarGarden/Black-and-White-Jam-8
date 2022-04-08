using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public bulletType thisBullet;
    public enum bulletType
    {
        normal = 0,
        fire = 1,
        electricity = 2,
        soap = 3,
        ice = 4,
        chip = 5,
        enemy = 6        
    }


    [SerializeField] private float selfDestructTimeInSeconds = 1;

    private void Start()
    {
        StartCoroutine(SelfDestruct());
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTimeInSeconds);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

    }
}
