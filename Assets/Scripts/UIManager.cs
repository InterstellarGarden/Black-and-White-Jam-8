using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider ui_combo;


    //POINTERS
    ComboBehaviour thisCombo;

    private void Awake()
    {
        //ASSIGN POINTERS
        thisCombo = FindObjectOfType<ComboBehaviour>();

        //POINTER PROPERTIES
        ui_combo.maxValue = thisCombo.maxCombo;
        ui_combo.value = 0;
    }

    void Update()
    {
        ui_combo.value = thisCombo.currentCombo;
    }
}
