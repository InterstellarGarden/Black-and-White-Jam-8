using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireBehaviour : MonoBehaviour
{
    public float durationBetweenFireInSec = 1, bulletSpeed = 1, delayBetweenEachRapidFireInSec = 0, angleBetweenEachBulletInCycle = 0;
    public int numberOfTimesToFirePerCycle = 1;
    private int fireIndex, secondaryFireIndex;
    private float currentAngle;
    bool firing;

    public Transform bulletSpawn;

    private GameObject player;
    [SerializeField] private GameObject bulletPrefab;
    void Start()
    {
        player = FindObjectOfType<CharacterBehaviour>().gameObject;

        //Give enemies some randomness in their firing behaviour
        fireIndex = Random.Range(0, 60);
    }
    
    private void FixedUpdate()
    {
        if (fireIndex >= durationBetweenFireInSec * 60)
        {
            if (!firing)
            {
                firing = true;
                GetComponent<EnemyBehaviour>().TriggerShootingAnim();
                StartCoroutine(FireCycle(numberOfTimesToFirePerCycle));
            }
        }
        fireIndex++;
    }
    public void Fire(float _rotation)
    {
        Vector3 _playerPos = player.transform.position;
        GameObject _bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
        _bullet.transform.LookAt(_playerPos);
        _bullet.transform.Rotate(0, _rotation, 0);

        _bullet.GetComponent<Rigidbody>().velocity = _bullet.transform.forward * bulletSpeed;
    }

    IEnumerator FireCycle(int _numberOfRapidFire)
    {
        currentAngle = -(angleBetweenEachBulletInCycle * (float)Mathf.Floor(numberOfTimesToFirePerCycle/2));
        while (firing)
        {
            Fire(currentAngle);

            currentAngle += angleBetweenEachBulletInCycle;
            secondaryFireIndex++;

            if (secondaryFireIndex >= numberOfTimesToFirePerCycle)
            {
                fireIndex = 0;
                secondaryFireIndex = 0;
                firing = false;
                yield return null;
            }

            yield return new WaitForSeconds(delayBetweenEachRapidFireInSec);
        }
    }
}
