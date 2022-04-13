using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;
    public static int shotsFired, shotsHit, enemiesKilled, LoopsReached;
    public static float accuracy;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void ShotFired()
    {
        
        shotsFired++;
        CalculateAccuracy();
    }

    public void ShotHit()
    {
        shotsHit++;
    }
    private void CalculateAccuracy()
    {
        accuracy = ((float)shotsHit / (float)shotsFired)*100;
    }
    public void Reset()
    {
        accuracy = 100;
        shotsFired = 0;
        shotsHit = 0;
        enemiesKilled = 0;
        LoopsReached = 0;
    }
}
