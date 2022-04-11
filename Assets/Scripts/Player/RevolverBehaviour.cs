using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBehaviour : MonoBehaviour
{
    //ESSENTIALS
    private ComboBehaviour thisCombo;
    CharacterBehaviour thisPlayer;

    public List<GameObject> bullets;
    public List<GameObject> bulletPrefabs, unlockableBulletPrefabs;
    public Transform bulletspawn,crouchingBulletSpawn;
    GameObject desiredBullet;

    public int numberOfTemporaryBullet;

    //FIRE RATE
    public float fireRate;
    float timeLastFired;

    //VISUAL
    public float bulletSpeed = 60;

    public GameObject poof;

    //TARGETABLE REFERS TO: WHAT WILL THE PLAYER'S BULLET COLLIDE WITH - WALLS, ENTITIES ETC.
    public LayerMask targettableMask;

    //UI ELEMENTS
    private uiRubiBehaviour thisRubiBehaviour;

    void Awake()
    {
        thisCombo = GetComponent<ComboBehaviour>();
        thisPlayer = GetComponent<CharacterBehaviour>();

        numberOfTemporaryBullet = 0;

        thisRubiBehaviour = FindObjectOfType<uiRubiBehaviour>();
    }
    private void Start()
    {
        //INITIALISE BULLETS INTO RUBI
        for (int i = 0; i < 6; i++)
        {
            thisRubiBehaviour.SetBullet(i, (int)bullets[i].GetComponent<BulletBehaviour>().thisBullet);
        }
    }
    void Update()
    {
        if (GameManager.playerIsDead)
            return;

        desiredBullet = bullets[0];

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timeLastFired + 1/fireRate)
        {
            timeLastFired = Time.time;

            #region Firing and Combo
            //PREPARE DATA FROM BULLET ABOUT TO BE FIRED
            int _currentType = (int)desiredBullet.GetComponent<BulletBehaviour>().thisBullet;

            //RAYCAST FROM CENTER OF SCREEN ONWARDS
            Vector3 _screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0));
            Vector3 _screenForward = Camera.main.transform.forward;

            RaycastHit _hitInfo;
            Debug.DrawRay(_screenCenter, _screenForward * 20, Color.red, 5);
            if (Physics.Raycast(_screenCenter, _screenForward, out _hitInfo, Mathf.Infinity, targettableMask))
            {
                Instantiate(poof, _hitInfo.point, Quaternion.identity);

                Debug.DrawLine(_screenCenter, _hitInfo.transform.position, Color.blue, 5);
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
            }


            //RESET COMBO REGARDLESS OF HITTING OR MISSING
            if (thisCombo.canInstantKill)
                thisCombo.TriggerRestartDecay();
            #endregion

            //UI
            thisRubiBehaviour.FiredBullet();
            FindObjectOfType<gunUiBehaviour>().Fire();

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
        if (numberOfTemporaryBullet > 0 )
        {
            numberOfTemporaryBullet--;
        }

        bullets.RemoveAt(0);
        if (bullets.Count < 3)
            TriggerReloadNextBullet();

        thisRubiBehaviour.RotateForward();
    }

    void TriggerReloadNextBullet()
    {
        int _chosenBullet = Random.Range(0, bulletPrefabs.Count);
        bullets.Add(bulletPrefabs[_chosenBullet]);

        thisRubiBehaviour.SetBullet(2, (int)bulletPrefabs[_chosenBullet].GetComponent<BulletBehaviour>().thisBullet);
    }
    public void TriggerPickUpBullet(int _bulletType)
    {
        GameObject _pickedUpBullet = bulletPrefabs[_bulletType];

        #region AddBullet
        if (numberOfTemporaryBullet > 0)
        {
            float _delta = 2 - numberOfTemporaryBullet;

            //Replace top, temporary bullet with new temporary bullet
            bullets[0] = _pickedUpBullet; 
            //Add one more bullet on top.
            for (int n = 0; n < _delta; n++)
            {
                bullets.Insert(0, _pickedUpBullet);
            }
        }

        else
        {
            //Add 2 temporary bullet on top of list of permanent bullets
            bullets.Insert(0, _pickedUpBullet); 
            bullets.Insert(0, _pickedUpBullet);
        }
        #endregion

        #region Remove Excess Bullets
        while (bullets.Count > 6)
        {
            //Remove bullets at tail end
            bullets.RemoveAt(6);
        }
        #endregion

        numberOfTemporaryBullet = 2;

        thisRubiBehaviour.AddBullet((int)_pickedUpBullet.GetComponent<BulletBehaviour>().thisBullet);
        thisRubiBehaviour.AddBullet((int)_pickedUpBullet.GetComponent<BulletBehaviour>().thisBullet);

        thisRubiBehaviour.RotateBackward();
        thisRubiBehaviour.RotateBackward();
    } 
    public void TriggerUnlockArsenal()
    {
        bulletPrefabs.Add(unlockableBulletPrefabs[0]);
        unlockableBulletPrefabs.RemoveAt(0);
    }
}
