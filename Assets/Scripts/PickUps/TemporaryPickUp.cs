using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPickUp : PickUP
{
    public List<GameObject> powerUpPrefabs;
    public enum types
    {
        tnt = 1,
        health = 2
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
        //Algorithm
        //Health is guaranteed to drop at 2 health. Otherwise, its always random

        _currentPickUp = (int)types.health;

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
