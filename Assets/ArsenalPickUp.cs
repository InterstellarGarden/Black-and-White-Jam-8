using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalPickUp : PickUP
{
    public int bulletToUnlock;
    public List<GameObject> unlockableBulletPrefabs;
    private void Start()
    {
        InitialiseBullet();
    }
    public void InitialiseBullet()
    {
        int _selected = bulletToUnlock;

        //VISUAL
        GameObject _toSpawn;
        _toSpawn = unlockableBulletPrefabs[_selected];        

        Instantiate(_toSpawn, transform);
    }
    protected override void OnPickUp()
    {
        FindObjectOfType<RevolverBehaviour>().TriggerUnlockArsenal();

        if (!FindObjectOfType<CombatManager>().isBossAlive)
            FindObjectOfType<CombatManager>().ForceEndCombat();
        base.OnPickUp();    
    }
}
