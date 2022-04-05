using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriageManager : MonoBehaviour
{
    public bool debugInitialiseCarriages;

    public List<GameObject> carriagePrefabs;

    //SPAWNING
    private List<GameObject> carriagesToSpawn;
    [SerializeField] private List<GameObject> spawnedCarriages;
    [SerializeField] private CarriageData currentCarriage, oldCarriage;
    [SerializeField] private Transform firstSpawn;
    [SerializeField] private Transform nextSpawnPos,prevSpawnPos;

    //COMBAT
    CombatManager thisCombatManager;

    //LOOP
    public static int loopsCompleted;

    private void Awake()
    {
        thisCombatManager = FindObjectOfType<CombatManager>();
    }
    private void Start()
    {
        if (debugInitialiseCarriages)
            InitialiseCarriages();
    }

    private void InitialiseCarriages()
    {
        //ALGORITHM CAN BE ALTERED TO BE RANDOMISED ON EVERY NEW PLAYTHROUGH OR KEPT AS IS

        carriagesToSpawn = new List<GameObject>(carriagePrefabs.Count);
        foreach (GameObject _carriage in carriagePrefabs)
            carriagesToSpawn.Add(_carriage);


        //PREPARE SPAWN FIRST CARRIAGE
        nextSpawnPos = firstSpawn;

        //NAMING
        int _count = 0;

        //SPAWN SUBSEQUENT CARRIAGES
        foreach (GameObject _carriage in carriagesToSpawn)
        {
            
            GameObject _spawned = Instantiate(_carriage, nextSpawnPos.position, nextSpawnPos.rotation);
            _spawned.GetComponent<CarriageData>().UpdateCarriageState(false);

            //NAMING
            _count++;
            _spawned.name = "Carriage " + (_count);
            _spawned.GetComponent<CarriageData>().InitialiseId(_count);

            spawnedCarriages.Add(_spawned);

            //UPDATE NEXT SPAWN
            nextSpawnPos = _spawned.GetComponent<CarriageData>().exitPos;
            
        }

        //INITIALISE CURRENT/OLD CARRIAGE SETTINGS
        currentCarriage = spawnedCarriages[2].GetComponent<CarriageData>();
        oldCarriage = currentCarriage;
        prevSpawnPos = spawnedCarriages[0].gameObject.transform;

    }
    public void UpdateCurrentCarriage(CarriageData _current)
    {
        currentCarriage = _current;
        Debug.Log("current: " + currentCarriage.carriageId + ". old: " + oldCarriage.carriageId);

        #region Moving Carriages
        //MOVE CARRIAGE FROM BACK TO FRONT
        //Whether new carriage is infront of old carriage, or new carriage is the highest value of the carriageID list possible.
        if ((currentCarriage.carriageId > oldCarriage.carriageId) && currentCarriage.carriageId != oldCarriage.carriageId)
        {
           
            GameObject _backCarriage = spawnedCarriages[0];

            _backCarriage.transform.position = nextSpawnPos.position;
            _backCarriage.transform.rotation = nextSpawnPos.rotation;
            
            spawnedCarriages.Add(_backCarriage.gameObject);
            _backCarriage.GetComponent<CarriageData>().carriageId+=spawnedCarriages.Count;
            nextSpawnPos = spawnedCarriages[spawnedCarriages.Count - 1].GetComponent<CarriageData>().exitPos;
            
            //NEW BACK CARRIAGE
            spawnedCarriages.RemoveAt(0);
            prevSpawnPos = spawnedCarriages[0].gameObject.transform;


            
            //Debug.Log("attempt move");
        }


        //MOVE CARRIAGE FROM BACK TO FRONT
        else if ((currentCarriage.carriageId < oldCarriage.carriageId) && currentCarriage.carriageId != oldCarriage.carriageId)
        {
            GameObject _frontCarriage = spawnedCarriages[spawnedCarriages.Count - 1];

            //GET WORLD VECTOR OFFSET FROM EXIT POSITION TO START POSITION
            _frontCarriage.transform.rotation = prevSpawnPos.rotation;

            Vector3 _offset =  _frontCarriage.transform.position - _frontCarriage.GetComponent<CarriageData>().exitPos.position;
            _frontCarriage.transform.position = prevSpawnPos.position + _offset  ;


            //ROTATE
            Quaternion _rotation = prevSpawnPos.rotation;
            


            //MOVE FRONT CARRIAGE TO 'BACK' OF LIST
            spawnedCarriages.Insert(0, _frontCarriage);
            _frontCarriage.GetComponent<CarriageData>().carriageId -= spawnedCarriages.Count;
            spawnedCarriages.RemoveAt(spawnedCarriages.Count - 1);
            nextSpawnPos = spawnedCarriages[spawnedCarriages.Count - 1].GetComponent<CarriageData>().exitPos;            
            prevSpawnPos = _frontCarriage.gameObject.transform;


            

            //Debug.Log("attempt move back");
        }
        #endregion

        //INITIATING COMBAT
        thisCombatManager.InitialiseCombat(_current);
        _current.UpdateCarriageState(true);

        //SET TO OLD CARRIAGE FOR COMPARISON WHEN ENTERING NEW CARRIAGE
        oldCarriage = currentCarriage;
    }
    public void UpdateIncreaseLoop()
    {
        loopsCompleted++;
    }
    private void OnDrawGizmos()
    {
        if (nextSpawnPos != null)
        {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(nextSpawnPos.position, 4);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(prevSpawnPos.position, 4);
        }
    }
}
