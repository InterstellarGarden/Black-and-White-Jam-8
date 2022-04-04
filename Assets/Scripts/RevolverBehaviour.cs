using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBehaviour : MonoBehaviour
{
    //ESSENTIALS
    private ComboBehaviour thisCombo;
    CharacterBehaviour thisPlayer;

     public List<GameObject> bullets;
    public List<GameObject> bulletPrefabs;
    public Transform bulletspawn,crouchingBulletSpawn;
    GameObject desiredBullet;

    public bool hasTemporaryBullet;
        
    //VISUAL
    public float bulletSpeed = 60;

    //TARGETABLE REFERS TO: WHAT WILL THE PLAYER'S BULLET COLLIDE WITH - WALLS, ENTITIES ETC.
    public LayerMask excludeTarget;

    void Awake()
    {
        thisCombo = GetComponent<ComboBehaviour>();
        thisPlayer = GetComponent<CharacterBehaviour>();

        hasTemporaryBullet = false;
    }

    void Update()
    {
        desiredBullet = bullets[0];

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            #region Firing and Combo
            //PREPARE DATA FROM BULLET ABOUT TO BE FIRED
            int _currentType = (int)desiredBullet.GetComponent<BulletBehaviour>().thisBullet;

            //RAYCAST FROM CENTER OF SCREEN ONWARDS
            Vector3 _screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0));
            Vector3 _screenForward = Camera.main.transform.forward;

            RaycastHit _hitInfo;
            if (Physics.Raycast(_screenCenter, _screenForward, out _hitInfo, Mathf.Infinity))
                if (_hitInfo.collider.TryGetComponent(out EntityBehaviour _enemy))
                {
                    //CHECK FOR INSTANT KILL
                    if (thisCombo.canInstantKill)
                        _enemy.TriggerInstantKill();
                    else
                        _enemy.TriggerTakeDamage(_currentType);
                }

            else if (_hitInfo.collider.TryGetComponent(out DestructibleBehaviour _destructible))
                {
                    //CHECK FOR INSTANT KILL
                    if (thisCombo.canInstantKill)
                        _destructible.TriggerInstantKill();
                    else
                        _destructible.TriggerTakeDamage(_currentType);
                }

                else Debug.Log("Error targetting entity");


            //RESET COMBO REGARDLESS OF HITTING OR MISSING
            if (thisCombo.canInstantKill)
                thisCombo.TriggerRestartDecay();
            #endregion

            //SPAWN BULLET - VISUAL ONLY
            TriggerSpawnBullet();

            //NEXT BULLET ON CHAMBER
            TriggerNextBullet();
        }
    }

    void TriggerSpawnBullet()
    {
        Vector3 _desiredPosition;
        switch (thisPlayer.isCrouching)
        {
            case true:
                _desiredPosition = crouchingBulletSpawn.transform.position;
                break;
            case false:
                _desiredPosition = bulletspawn.transform.position;
                break;
        }

        GameObject _bulletClone = Instantiate(desiredBullet, _desiredPosition, Quaternion.identity, null);
        _bulletClone.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;
    }

    void TriggerNextBullet()
    {        
        if (hasTemporaryBullet)
        {            
            hasTemporaryBullet = false;
            FindObjectOfType<dump_BulletUI>().removeTempBullet();
        }

        bullets.RemoveAt(0);
        if (bullets.Count <= 0)
            TriggerReloadChamber();
    }

    void TriggerReloadChamber()
    {
        for (int n = 0; n < 6; n++)
        {
            bullets.Add(bulletPrefabs[Random.Range(0, bulletPrefabs.Count)]);
        }
    }
    public void TriggerPickUpBullet(int _bulletType)
    {
        GameObject _pickedUpBullet = bulletPrefabs[_bulletType];

        if (hasTemporaryBullet)
            bullets[0] = _pickedUpBullet; //Replace top, temporary bullet with new temporary bullet

        else
            bullets.Insert(0, _pickedUpBullet); //Add a temporary bullet on top of list of permanent bullets

        hasTemporaryBullet = true;
    }
}
