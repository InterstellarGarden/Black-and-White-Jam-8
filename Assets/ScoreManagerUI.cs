using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManagerUI : MonoBehaviour
{
    public TMP_Text accuracy, enemiesKilled, loopsReached;
    void Start()
    {
        accuracy.text = Mathf.FloorToInt(ScoreManager.accuracy) + "%";
        enemiesKilled.text = ""+ScoreManager.enemiesKilled;
        loopsReached.text = ""+(ScoreManager.LoopsReached +1);
    }

}
