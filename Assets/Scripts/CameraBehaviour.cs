using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    private Transform oriPos,camCrouchPos;

    private CharacterBehaviour thisPlayer;

    private void Awake()
    {
        thisPlayer = FindObjectOfType<CharacterBehaviour>();
    }
    void Update()
    {
        switch (thisPlayer.isCrouching)
        {
            case true:
                transform.position = camCrouchPos.position;
                break;

            case false:
                transform.position = oriPos.position;
                break;
        }
    }
}
