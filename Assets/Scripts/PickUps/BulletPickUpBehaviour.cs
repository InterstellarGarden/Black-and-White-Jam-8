using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUpBehaviour : PickUP
{
    public List<GameObject> bulletPrefabs;
    public int bulletType;
    private void Start()
    {
        InitialiseBullet();
    }
    void InitialiseBullet()
    {
        int _selected = bulletType;

        switch (_selected)
        {
            default: //reggie
                bulletType = (int)BulletBehaviour.bulletType.normal;
                break;
            case 1: //babylegs
                bulletType = (int)BulletBehaviour.bulletType.fire;
                break;
            case 2:
                bulletType = (int)BulletBehaviour.bulletType.electricity;
                break;
            case 3:
                bulletType = (int)BulletBehaviour.bulletType.soap;
                break;

        }

        GameObject _pickUp = Instantiate(bulletPrefabs[bulletType], transform);        
    }
    protected override void OnPickUp()
    {
        FindObjectOfType<RevolverBehaviour>().TriggerPickUpBullet(bulletType);
        base.OnPickUp();
    }
}
