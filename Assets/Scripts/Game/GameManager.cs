using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool playerIsDead;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        playerIsDead = false;
    }
    public void StartGame()
    {

    }
    
    public void GameOver()
    {
        Debug.Log("GameOver");
        playerIsDead = true;
        FindObjectOfType<CharacterBehaviour>().GameOver();
    }
}
