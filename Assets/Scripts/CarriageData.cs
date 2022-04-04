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
        entryDoor.SetActive(_isInCombat);
        exitDoor.SetActive(_isInCombat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EFFECTS ON ENTERING NEW CARRIAGE
            thisManager.UpdateCurrentCarriage(this);
        }
    }
}
