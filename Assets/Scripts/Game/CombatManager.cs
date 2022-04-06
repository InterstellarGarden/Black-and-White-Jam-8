using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    bool inCombat = false;
    public int enemiesToSpawn;

    [SerializeField]
    private List<GameObject> enemyPrefabs,activeEnemies;
    private List<Vector3> spawnPositions, chosenSpawnPositions;

    private CarriageData currentCarriage;

    private void Awake()
    {
        inCombat = false;
    }
    
    public void InitialiseCombat(CarriageData _currentCarriage)
    {
        if (inCombat)
            return;

        inCombat = true;
        currentCarriage = _currentCarriage;

        //CHOOSE SPAWNERS
        //Initialise Spawners
        spawnPositions = new List<Vector3>();
        foreach (Transform _spawner in currentCarriage.enemySpawners)
            spawnPositions.Add(_spawner.position);

        chosenSpawnPositions = new List<Vector3>();
        //Algorithm to select spawners to spawn on
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
    }
    public bool CheckForEndCombat(GameObject _enemyToRemove)
    {
        if (activeEnemies.Contains(_enemyToRemove))
        {
            activeEnemies.Remove(_enemyToRemove);
            if (activeEnemies.Count <= 0)
            {
                //Algorithm can be added here to roll for additional enemy reinforcements

                EndCombat();
                return true;
            }

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
        currentCarriage.UpdateCarriageState(false);
        spawnPositions.Clear();
        chosenSpawnPositions.Clear();

        //Roll for chance to spawn powerups or pickups
    }

    public void TriggerSpawnPickUp(Vector3 _position)
    {
        GameObject _prefab = Resources.Load<GameObject>("TemporaryPickUp");
        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}