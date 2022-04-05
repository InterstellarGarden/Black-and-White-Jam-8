using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPickUp : MonoBehaviour
{
    public List<GameObject> powerUpPrefabs;
    public enum types
    {
        tnt = 1,
        speedUp = 2,
        fastFire = 3
    }
    public int _currentPickUp;
    void Start()
    {
        InitialisePowerUp();
    }
    void InitialisePowerUp()
    {
        int _selected = Random.Range(2, 3);

        switch (_selected)
        {
            case 2:
                _currentPickUp = (int)types.speedUp;
                break;
            case 3:
                _currentPickUp = (int)types.fastFire;
                break;
        }
        //Convert human-friendly bulletType names into programming-friendly index (For Lists and correct pointers)

    }    
}
