using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public BulletBehaviour.bulletType thisWeakness;
    [SerializeField] protected int maxHealth;
    [SerializeField] private int health;
    protected virtual void Awake()
    {
        health = maxHealth;
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
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }

        //CHANCE TO DROP BULLET
        else
        {
            GameObject _prefab = Resources.Load<GameObject>("BulletPickUp");
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }

        //DEATH ANIMATION AND SOUND CAN BE INSERTED HERE

        Destroy(gameObject);
    }
}
