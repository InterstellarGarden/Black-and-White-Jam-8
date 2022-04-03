using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    [SerializeField] private int enemyType;

    [SerializeField] private int health;
    

    public void Trigger_TakeDamage(int _bulletType)
    {
        //TAKE DAMAGE
        if (_bulletType == enemyType)
            Trigger_InstantKill();

        else health--;

        //CHECK FOR HEALTH/DEATH
        if (health <= 0)
            Death();

        else
        {
            //TAKING HIT ANIMATION AND SOUND CAN BE INSERTED HERE
        }

    }

    public void Trigger_InstantKill()
    {
        Death();
    }

    void Death()
    {
        //ADD COMBO
        FindObjectOfType<ComboBehaviour>().Trigger_AddCombo(1);


        //DEATH ANIMATION AND SOUND CAN BE INSERTED HERE

        Destroy(gameObject);
    }
}
