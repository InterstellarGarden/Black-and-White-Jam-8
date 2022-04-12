using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool playerIsDead;

    bool killfloorCheck = true;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        playerIsDead = false;
        killfloorCheck = true;
    }
    private void Start()
    {
        ScoreManager.instance.Reset();
        StartCoroutine(CoroKillfloor());
    }
    IEnumerator CoroKillfloor()
    {
        while (killfloorCheck)
        {
            foreach (EntityBehaviour _entity in FindObjectsOfType<EntityBehaviour>())
            {
                Vector3 _pos = _entity.transform.position;
                if (_pos.y <= FindObjectOfType<CarriageManager>().firstSpawn.transform.position.y)
                    _entity.Death();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void StartGame()
    {

    }
    
    public void GameOver()
    {
        Debug.Log("GameOver");
        playerIsDead = true;
        FindObjectOfType<CharacterBehaviour>().GameOver();
        FindObjectOfType<TransitionManager>().LoadScene(2);
    }
}
