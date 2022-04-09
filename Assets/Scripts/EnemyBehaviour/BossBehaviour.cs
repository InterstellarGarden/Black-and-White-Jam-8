using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    [SerializeField] private float intervalBetweenRespawnEnemies = 10;
    public float distanceFromPlayerToSpawnArsenal = 5;
    protected override void Start()
    {
        thisCombatManager = FindObjectOfType<CombatManager>();
        thisCombatManager.isBossAlive = true;
        SpawnArsenal(CarriageManager.loopsCompleted);
        StartCoroutine(CallEnemies());
        base.Start();
    }

    public override void Death()
    {
        if (!thisCombatManager.isBossAlive)
            return;

        thisCombatManager.isBossAlive = false;
        StopAllCoroutines();

        thisCombatManager.ClearEnemies();
        FindObjectOfType<CarriageManager>().UpdateIncreaseLoop();
        SpawnArsenal(CarriageManager.loopsCompleted);
        base.Death();   
    }
    public void SpawnArsenal(int _choice)
    {
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
