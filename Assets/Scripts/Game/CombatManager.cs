using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    bool inCombat = false;
    private int enemiesToSpawn;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> unlockableEnemyPrefabs;
    public enum UnlockableEnemies
    {
        electric = 0,
        soap = 1,
        ice = 2,
        chip = 3
    }
    public List<GameObject> activeEnemies;
    private List<Vector3> spawnPositions, chosenSpawnPositions;

    //Repeatable Enemy Spawners
    public int maxEnemiesAtATime;
    [HideInInspector] public bool isBossAlive = false;

    //Counting Enemies
    public List<int> numberOfEachActiveEnemyType;

    //SOUND
    [Range(0, 1)] [SerializeField] private float sfxMultiplier;
    [SerializeField] private List<AudioClip> enemyYeehaw;
    [SerializeField] private AudioClip bossYeehaw, whistle;


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

        //MUSIC
        FindObjectOfType<MusicManager>().RequestHorizontalLayer((int)MusicManager.music.combat);
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

        enemiesToSpawn = (currentCarriage.numberOfEnemiesToSpawn - activeEnemies.Count);
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

            //PLAY SOUND 
            if (currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Vault && isBossAlive)
                FindObjectOfType<SoundManager>().TriggerPlaySound(whistle, sfxMultiplier/2, true);

            FindObjectOfType<SoundManager>().TriggerPlaySound(enemyYeehaw[Random.Range(0, enemyYeehaw.Count)], sfxMultiplier, true);
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

            FindObjectOfType<SoundManager>().TriggerPlaySound(bossYeehaw, sfxMultiplier, false);
        }
        #endregion
    }
    public void BossSpawnEnemies()
    {
        SpawnEnemies();
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
                if (currentCarriage._isSpecialCarriage == CarriageData.SpecialCarriageExceptions.Vault)
                {
                    //'End Combat' ie. open doors after picking up arsenal
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

        FindObjectOfType<MusicManager>().RequestHorizontalLayer((int)MusicManager.music.calm);
    }
    public void ForceEndCombat()
    {
        if (!isBossAlive)
            EndCombat();
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

    public void ClearEnemies()
    {
        for (int i = 0; i < numberOfEachActiveEnemyType.Count-1; i++)
        {
            numberOfEachActiveEnemyType[i] = 0;
        }

        foreach (GameObject _enemy in activeEnemies)
            Destroy(_enemy);

        activeEnemies.Clear();
    }

    public EntityBehaviour.enemyType CountHighestEnemy()
    {
        int _highest = 0;
        int _enemyChoice = 0;
        for (int i =0; i < 6; i++)
        {
            _highest = Mathf.Max(_highest, numberOfEachActiveEnemyType[i]);            
        }        

        for (int n = 0; n <6; n++)
        {
            if (numberOfEachActiveEnemyType[n] == _highest)
            {
                _enemyChoice = n;
                //Debug.Log("enemyChoice: " + n);
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
    public void TriggerNewEnemyType(int _choice)
    {
        if (_choice > unlockableEnemyPrefabs.Count)
        {
            Debug.Log("No more enemies to unlock");
            return;
        }

        else if (unlockableEnemyPrefabs[_choice] != null)
            enemyPrefabs.Add(unlockableEnemyPrefabs[_choice]);
    }
}
