using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriageData : MonoBehaviour
{
    [HideInInspector] public int carriageId;

    [SerializeField] public GameObject entryDoor, exitDoor;
    public Transform exitPos;

    private BoxCollider playerDetect;
    private CarriageManager thisManager;
    [HideInInspector] public List<Transform> enemySpawners;

    public int numberOfEnemiesToSpawn, totalEnemiesPool;
    public enum SpecialCarriageExceptions
    {
        No = 0,
        Furnace = 1,
        Vault = 2,
        Conductor = 3
    }
    [SerializeField] public SpecialCarriageExceptions _isSpecialCarriage;

    private void Awake()
    {
        playerDetect = GetComponent<BoxCollider>();
        thisManager = FindObjectOfType<CarriageManager>();

        enemySpawners = new List<Transform>();
        foreach (Transform _child in transform)
            if (_child.TryGetComponent(out EnemySpawner _spawner))
                enemySpawners.Add(_spawner.transform);
    }

    public void InitialiseId(int _id)
    {
        carriageId = _id;
    }

    public void UpdateCarriageState(bool _isInCombat)
    {
        //Cannot open Vault doors normally without TNT        
        entryDoor.SetActive(_isInCombat);
        exitDoor.SetActive(_isInCombat);
    }
    
    public void ForceUpdateCarriageState(bool _isInCombat)
    {
        entryDoor.SetActive(_isInCombat);
        exitDoor.SetActive(_isInCombat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EFFECTS ON ENTERING NEW CARRIAGE
            //UNLOCK NEW ENEMY
            if (_isSpecialCarriage == SpecialCarriageExceptions.Furnace && CarriageManager.loopsCompleted < 1)
                FindObjectOfType<CombatManager>().TriggerNewEnemyType(CarriageManager.loopsCompleted);

            else if (_isSpecialCarriage == SpecialCarriageExceptions.Vault && CarriageManager.loopsCompleted < 1)
                FindObjectOfType<CombatManager>().TriggerNewEnemyType(CarriageManager.loopsCompleted + 1);

            //COMBAT
            thisManager.UpdateCurrentCarriage(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player") && _isSpecialCarriage == SpecialCarriageExceptions.Vault)
        //    ForceUpdateCarriageState(true);
    }
}
