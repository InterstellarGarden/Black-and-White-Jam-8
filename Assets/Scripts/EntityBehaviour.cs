using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public BulletBehaviour.bulletType thisWeakness;
    [SerializeField] protected int maxHealth;
    [SerializeField] private int health;
    public enum enemyType
    {
        reggie = 0,
        babylegs = 1,
        bigGuy = 2,
        filthy = 3,
        drunk = 4,
        gambler = 5,
        boss = 6
    }
    protected CombatManager thisCombatManager;
    protected virtual void Awake()
    {
        health = maxHealth;
        thisCombatManager = FindObjectOfType<CombatManager>();
    }
    public void TriggerTakeDamage(int _bulletType)
    {
        //TAKE DAMAGE
        if (_bulletType == (int)thisWeakness)
            TriggerInstantKill();

        else health--;

        //CHECK FOR HEALTH/DEATH
        if (health <= 0)
            Death();

        else
        {
            //TAKING HIT ANIMATION AND SOUND CAN BE INSERTED HERE
        }
    }

    public void TriggerInstantKill()
    {
        Death();
    }

    public virtual void Death()
    {
        //ADD COMBO
        FindObjectOfType<ComboBehaviour>().TriggerAddCombo(1);

        //CHECK FOR END OF COMBAT - Spawn pick ups at end of carriage
        if (FindObjectOfType<CombatManager>().CheckForEndCombat(gameObject))
        {
            GameObject _prefab = Resources.Load<GameObject>("TemporaryPickUp");
            //Spawn TNT pick up at 100% at Furnace room
            if (FindObjectOfType<CombatManager>().currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Furnace)
            {
                _prefab.GetComponent<TemporaryPickUp>().isTnt = true;
                Instantiate(_prefab, transform.position, Quaternion.identity);
            }               

            //Spawn HP pick up at 100% at end of each other room if player is low health
            else if (FindObjectOfType<CharacterBehaviour>().isLowHealth)
            {
                _prefab.GetComponent<TemporaryPickUp>().isTnt = false;
                Instantiate(_prefab, transform.position, Quaternion.identity);
            }

            //Spawn HP pick up at 40% at end of each other room
            else
            {
                if (Random.value > 0.6f)
                {
                    _prefab.GetComponent<TemporaryPickUp>().isTnt = false;
                    Instantiate(_prefab, transform.position, Quaternion.identity);
                }               
            }
        }

        //CHANCE TO DROP BULLET - Only in the middle of combat
        else
        {
            //Roll 1-100s
            if (Random.value > 0.75)
            {
                //LOAD INITIAL PREFAB FROM RESOURCE
                GameObject _prefab = Resources.Load<GameObject>("BulletPickUp");

                //ALGORITHM TO SELECT BULLET
                enemyType _enemyChosen = thisCombatManager.CountHighestEnemy();
                int _idealBullet = (int)_enemyChosen;

                //APPLY RESULT AND INSTANTIATE BULLET
                _prefab.GetComponent<BulletPickUpBehaviour>().bulletType = _idealBullet;

                //DON'T SPAWN NEW BULLET PICKUP IF NOT IN ARSENAL YET
                if (!FindObjectOfType<RevolverBehaviour>().bulletPrefabs.Contains(_prefab))
                    return;

                Instantiate(_prefab, transform.position, Quaternion.identity);
            }
        }

        //DEATH ANIMATION AND SOUND CAN BE INSERTED HERE

        Destroy(gameObject);
    }
}
