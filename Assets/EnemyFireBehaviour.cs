using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireBehaviour : MonoBehaviour
{
    public float durationBetweenFireInSec = 1, bulletSpeed = 1, delayBetweenEachRapidFireInSec = 0;
    public int numberOfTimesToFirePerCycle = 1;
    private int fireIndex, secondaryFireIndex;
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
                StartCoroutine(FireCycle(numberOfTimesToFirePerCycle));
            }
        }
        fireIndex++;
    }
    public void Fire()
    {
        Vector3 _playerPos = player.transform.position;
        GameObject _bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
        _bullet.transform.LookAt(_playerPos);
        _bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * bulletSpeed;
    }

    IEnumerator FireCycle(int _numberOfRapidFire)
    {
        while (firing)
        {
            Fire();            
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
