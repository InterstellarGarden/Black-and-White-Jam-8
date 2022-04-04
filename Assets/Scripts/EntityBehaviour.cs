using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public bulletWeakness thisWeakness;

    public enum bulletWeakness
    {
        bullet1 = 1,
        bullet2 = 2
    }
    [SerializeField] private int health;
    

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

        //CHECK FOR END OF COMBAT
        FindObjectOfType<CombatManager>().CheckForEndCombat(gameObject);

        //CHANCE TO DROP BULLET
        GameObject _prefab = Resources.Load<GameObject>("BulletPickUp");
        Instantiate(_prefab, transform.position, Quaternion.identity);

        //DEATH ANIMATION AND SOUND CAN BE INSERTED HERE

        Destroy(gameObject);
    }
}
