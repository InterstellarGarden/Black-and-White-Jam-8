using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    [SerializeField] private float intervalBetweenRespawnEnemies = 10;
    public float distanceFromPlayerToSpawnArsenal = 5;
    private static int numArsenalUnlocked = 0;

    //SOUNDS
    [SerializeField] private List<AudioClip> shot;
    protected override void Start()
    {
        thisCombatManager = FindObjectOfType<CombatManager>();
        thisCombatManager.isBossAlive = true;
        SpawnArsenal(CarriageManager.loopsCompleted);
        StartCoroutine(CallEnemies());
        base.Start();
    }
    public override void TriggerTakeDamage(int _bulletType)
    {
        AudioClip _chosenSound = shot[Random.Range(0, shot.Count)];
        FindObjectOfType<SoundManager>().TriggerPlaySound(_chosenSound, sfxMultiplier);

        base.TriggerTakeDamage(_bulletType);
    }
    public override void Death()
    {
        if (!thisCombatManager.isBossAlive)
            return;

        thisCombatManager.isBossAlive = false;
        StopAllCoroutines();

        thisCombatManager.ClearEnemies();

        //FOR OPENING VAULT BEYOND ARSENAL UNLOCKS
        if (FindObjectOfType<RevolverBehaviour>().unlockableBulletPrefabs.Count <= 0)
            thisCombatManager.ForceEndCombat();

        FindObjectOfType<CarriageManager>().UpdateIncreaseLoop();

        SpawnArsenal(CarriageManager.loopsCompleted);

        FindObjectOfType<CarriageManager>().UpdateVaultOpened(false);
        base.Death();   
    }
    public void SpawnArsenal(int _choice)
    {
        //VERY RUDIMENTARY FIX
        //Basically, since there are only 2 new weapons, if it goes beyond 2, then no more weapons to unlock 
        if (FindObjectOfType<RevolverBehaviour>().unlockableBulletPrefabs.Count <= 0)
            return;

        GameObject _arsenal = Resources.Load<GameObject>("ArsenalPickUp");
        GameObject _player = FindObjectOfType<CharacterBehaviour>().gameObject;
        _arsenal.GetComponent<ArsenalPickUp>().bulletToUnlock = _choice;
        Instantiate(_arsenal, _player.transform.position + (_player.transform.forward * distanceFromPlayerToSpawnArsenal), Quaternion.identity);
    }
    IEnumerator CallEnemies()
    {
        while (thisCombatManager.isBossAlive)
        {
            yield return new WaitForSeconds(intervalBetweenRespawnEnemies);
            thisCombatManager.BossSpawnEnemies();
        }
    }
}
