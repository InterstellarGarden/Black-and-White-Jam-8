using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public bulletType thisBullet;
    public enum bulletType
    {
        normal = 1,
        fire = 2
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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

    }
}
