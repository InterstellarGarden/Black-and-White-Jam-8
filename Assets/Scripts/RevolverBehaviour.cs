using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBehaviour : MonoBehaviour
{
    //ESSENTIALS
    private ComboBehaviour thisCombo;


    public List<GameObject> bullets;
    public Transform bulletspawn;
    GameObject desiredBullet;
    

    //VISUAL
    public float bulletSpeed = 60;

    //TARGETABLE REFERS TO: WHAT WILL THE PLAYER'S BULLET COLLIDE WITH - WALLS, ENTITIES ETC.
    public LayerMask excludeTarget;

    void Awake()
    {
        thisCombo = GetComponent<ComboBehaviour>();
    }

    void Update()
    {
        desiredBullet = bullets[0];

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            #region Firing and Combo
            //PREPARE DATA FROM BULLET ABOUT TO BE FIRED
            int _currentType = desiredBullet.GetComponent<BulletBehaviour>().bulletType;

            //RAYCAST FROM CENTER OF SCREEN ONWARDS
            Vector3 _screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0));
            Vector3 _screenForward = Camera.main.transform.forward;

            RaycastHit _hitInfo;
            if (Physics.Raycast(_screenCenter, _screenForward, out _hitInfo, Mathf.Infinity))
                if (_hitInfo.collider.TryGetComponent(out EntityBehaviour _enemy))
                {
                    //CHECK FOR INSTANT KILL
                    if (thisCombo.canInstantKill)
                        _enemy.Trigger_InstantKill();
                    else
                        _enemy.Trigger_TakeDamage(_currentType);
                }


            //RESET COMBO REGARDLESS OF HITTING OR MISSING
            if (thisCombo.canInstantKill)
                thisCombo.Trigger_RestartDecay();
            #endregion

            //SPAWN BULLET - VISUAL ONLY
            Trigger_SpawnBullet();

            //NEXT BULLET ON CHAMBER
            Trigger_NextBullet();
        }
    }

    void Trigger_SpawnBullet()
    {
        GameObject _bulletClone = Instantiate(desiredBullet, bulletspawn.position, Quaternion.identity, null);
        _bulletClone.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;
    }

    void Trigger_NextBullet()
    {        
        bullets.Add(desiredBullet);
        bullets.RemoveAt(0);        
    }

}
