using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider ui_combo;
    [SerializeField] private TMP_Text hasTnt;

    //POINTERS
    ComboBehaviour thisCombo;
    CharacterBehaviour thisPlayer;

    private void Awake()
    {
        //ASSIGN POINTERS
        thisCombo = FindObjectOfType<ComboBehaviour>();
        thisPlayer = FindObjectOfType<CharacterBehaviour>();

        //POINTER PROPERTIES
        ui_combo.maxValue = thisCombo.maxCombo;
        ui_combo.value = 0;
    }

    void Update()
    {
        ui_combo.value = thisCombo.currentCombo;
        hasTnt.text = "hasTnt: " + CarriageManager.playerHasTnt;
    }
}
