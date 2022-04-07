using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    bool inCombat = false;
    private int enemiesToSpawn;
    [SerializeField] private List<GameObject> enemyPrefabs;
    public List<GameObject> activeEnemies;
    private List<Vector3> spawnPositions, chosenSpawnPositions;

    //Repeatable Enemy Spawners
    public int maxEnemiesAtATime;
    [HideInInspector] public bool isBossAlive = false;

    //Counting Enemies
    public List<int> numberOfEachActiveEnemyType;

    //Data
    [HideInInspector] public CarriageData currentCarriage;
    private CarriageData oldCarriage;
    private CarriageManager thisCarriageManager;

    private void Awake()
    {
        inCombat = false;
        thisCarriageManager = FindObjectOfType<CarriageManager>();

    }
    
    public void InitialiseCombat(CarriageData _currentCarriage)
    {
        if (inCombat)
            return;
        inCombat = true;
        currentCarriage = _currentCarriage;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        #region SpawnNormalEnemies
        //CHOOSE SPAWNERS
        //Initialise Spawners
        spawnPositions = new List<Vector3>();
        foreach (Transform _spawner in currentCarriage.enemySpawners)
            spawnPositions.Add(_spawner.position);

        chosenSpawnPositions = new List<Vector3>();
        //Algorithm to select spawners to spawn on

        enemiesToSpawn = currentCarriage.numberOfEnemiesToSpawn;
        for (int n = 1; n <= enemiesToSpawn; n++)
        {
            int _selected = Random.Range(0, spawnPositions.Count);
            chosenSpawnPositions.Add(spawnPositions[_selected]);
            spawnPositions.RemoveAt(_selected);
        }

        //SPAWN ENEMIES
        //Choose Enemies
        foreach (Vector3 _pos in chosenSpawnPositions)
        {
            int _selectedEnemy = Random.Range(0, enemyPrefabs.Count);
            GameObject _spawnedEnemy = Instantiate(enemyPrefabs[_selectedEnemy], _pos, Quaternion.identity); //MAY WANT TO REVISIT THIS LINE TO ADJUST FOR SPAWNING WITH ROTATION

            //ASSIGN TO ACTIVE ENEMIES
            activeEnemies.Add(_spawnedEnemy);
        }
        #endregion

        #region SpawnBoss
        if (currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Vault && !isBossAlive)
        {
            //CHOOSE SPAWNER
            Vector3 _bossSpawnPosition = currentCarriage.transform.Find("BossSpawner").transform.position;

            //SPAWN ENEMY
            GameObject _boss = Resources.Load<GameObject>("Boss");
            Instantiate(_boss, _bossSpawnPosition, Quaternion.identity);
        }
        #endregion
    }
    public bool CheckForEndCombat(GameObject _enemyToRemove)
    {
        if (activeEnemies.Contains(_enemyToRemove))
        {
            activeEnemies.Remove(_enemyToRemove);
            if (activeEnemies.Count <= 0)
            {
                //Algorithm can be added here to roll for additional enemy reinforcements

                //Exception for Vault Room
                //If currently in vault room; all filler enemies are dead; boss will spawn additional enemies (ie. combat is not over hence false)
                if (currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Vault && isBossAlive)
                {
                    SpawnEnemies();
                    return false;
                }

                //If no more enemies to spawn, combat is over.
                EndCombat();
                return true;
            }
            //This is basically just to satisfy the code's requirement to give all paths a way to return a value.
            else return false;
        }
        else
        {

            Debug.Log("Error, enemy is not considered as part of active enemy.");            
            return false;
        }
    }
    void EndCombat()
    {
        inCombat = false;

        if (currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Vault)
            currentCarriage.ForceUpdateCarriageState(false);

        else 
            currentCarriage.UpdateCarriageState(false);

        spawnPositions.Clear();
        chosenSpawnPositions.Clear();

        oldCarriage = currentCarriage;
    }

    //When boss spawns in, update this, when boss dies update this again
    public void UpdateBossState(bool _state)
    {
        isBossAlive = _state;
    }
    public void UpdateCountOfEachEnemyType(int _delta, int _enemyType)
    {
        numberOfEachActiveEnemyType[_enemyType] += _delta;
    }
    public EntityBehaviour.enemyType CountHighestEnemy()
    {
        int _highest = 0;
        int _enemyChoice = 0;
        for (int i =0; i < 6; i++)
        {
            _highest = Mathf.Max(_highest, numberOfEachActiveEnemyType[i]);
            //Debug.Log("i: " + i);
            //Debug.Log("currenthighest: " + _highest);
        }
        //Debug.Log("final highest: " + _highest);

        for (int n = 0; n <6; n++)
        {
            if (numberOfEachActiveEnemyType[n] == _highest)
            {
                _enemyChoice = n;
                Debug.Log("enemyChoice: " + n);
                break;
            }
        }
        switch (_enemyChoice)
        {
            default:
                return EntityBehaviour.enemyType.reggie;
            case (int)EntityBehaviour.enemyType.babylegs:
                return EntityBehaviour.enemyType.babylegs;
            case (int)EntityBehaviour.enemyType.bigGuy:
                return EntityBehaviour.enemyType.bigGuy;
            case (int)EntityBehaviour.enemyType.filthy:
                return EntityBehaviour.enemyType.filthy;
            case (int)EntityBehaviour.enemyType.drunk:
                return EntityBehaviour.enemyType.drunk;
            case (int)EntityBehaviour.enemyType.gambler:
                return EntityBehaviour.enemyType.gambler;
        }
    }
}
