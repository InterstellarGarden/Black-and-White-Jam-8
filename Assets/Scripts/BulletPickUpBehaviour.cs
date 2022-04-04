using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUpBehaviour : PickUP
{
    public List<GameObject> bulletPrefabs;
    private int _bulletType;
    private void Start()
    {
        InitialiseBullet();
    }
    void InitialiseBullet()
    {
        //ALGORITHM TO SELECT BULLET - Basic random bullet drops for now
        //Choose between 0 and 1
        int _selected = Random.Range(1,3); 

        switch (_selected)
        {
            case 1:
                _bulletType = (int)BulletBehaviour.bulletType.bullet1;
                break;
            case 2:
                _bulletType = (int)BulletBehaviour.bulletType.bullet2;
                break;
        }
        //Convert human-friendly bulletType names into programming-friendly index (For Lists and correct pointers)
        _bulletType -= 1;

        GameObject _pickUp = Instantiate(bulletPrefabs[_bulletType], transform);        
    }
    protected override void OnPickUp()
    {
        FindObjectOfType<RevolverBehaviour>().TriggerPickUpBullet(_bulletType);
        base.OnPickUp();
    }
}
