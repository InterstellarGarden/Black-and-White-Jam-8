using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPickUp : PickUP
{
    public List<GameObject> powerUpPrefabs;
    public enum types
    {
        tnt = 1,
        speedUp = 2,
        fastFire = 3,
        health = 4
    }
    public int _currentPickUp;
    public bool isTnt = false;
    void Start()
    {
        if (!isTnt)
            InitialisePowerUp();

        else InitialiseTnt();
    }
    void InitialisePowerUp()
    {
        int _selected = Random.Range(2, 5);

        //Algorithm
        //Health is guaranteed to drop at 2 health. Otherwise, its always random

        switch (_selected)
        {
            case 2:
                _currentPickUp = (int)types.speedUp;
                break;
            case 3:
                _currentPickUp = (int)types.fastFire;
                break;
            case 4:
                _currentPickUp = (int)types.health;
                break;
        }

        //Visual
        GameObject _pickUp = Instantiate(powerUpPrefabs[_currentPickUp - 1], transform);
    }

    void InitialiseTnt()
    {
        _currentPickUp = (int)types.tnt;
        GameObject _pickUp = Instantiate(powerUpPrefabs[_currentPickUp - 1], transform);
    }

    protected override void OnPickUp()
    {
        FindObjectOfType<CharacterBehaviour>().TriggerPowerUp(_currentPickUp);
        base.OnPickUp();
    }
}
