using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider ui_combo,ui_stamina;


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

        ui_stamina.maxValue = thisPlayer.maxStamina;
        ui_stamina.value = ui_stamina.maxValue;
    }

    void Update()
    {
        ui_combo.value = thisCombo.currentCombo;
        ui_stamina.value = thisPlayer.currentStamina;
    }
}
