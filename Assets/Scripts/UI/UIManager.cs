using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider uiCombo, uiHealth;
    [SerializeField] private TMP_Text uiLoop;
    [SerializeField] private Image uiTnt;

    bool oldHasTnt;

    //POINTERS
    ComboBehaviour thisCombo;
    CharacterBehaviour thisPlayer;

    private void Awake()
    {
        //ASSIGN POINTERS
        thisCombo = FindObjectOfType<ComboBehaviour>();
        thisPlayer = FindObjectOfType<CharacterBehaviour>();

        //POINTER PROPERTIES
        uiCombo.maxValue = thisCombo.maxCombo;
        uiCombo.value = 0;
        uiHealth.maxValue = thisPlayer.maxHealth;        
    }

    void Update()
    {
        uiCombo.value = thisCombo.currentCombo;
        uiHealth.value = thisPlayer.health;
        uiLoop.text = "Loop: " + (CarriageManager.loopsCompleted + 1);
    }
    public void UpdateTntUi(bool _hasTnt)
    {
        switch (_hasTnt)
        {
            case true:
                Color _color = uiTnt.color;
                _color.a = 1;
                uiTnt.color = _color;
                break;

            case false:
                Color _color2 = uiTnt.color;
                _color2.a = 0;
                uiTnt.color = _color2;
                break;
        }
    }
}
