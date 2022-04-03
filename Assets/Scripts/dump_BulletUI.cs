using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dump_BulletUI : MonoBehaviour
{
    RevolverBehaviour playerRevolver;

    public List<TMP_Text> texts;
    void Start()
    {
        playerRevolver = FindObjectOfType<RevolverBehaviour>();
    }

    void Update()
    {
        for (int i = 0; i <= playerRevolver.bullets.Count - 1; i++)
        {
            texts[i].text = playerRevolver.bullets[i].name;
        }
    }
}
