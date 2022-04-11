using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehaviour : MonoBehaviour
{
    public static bool isNotFirstTimePlaying = false;
    public Transform pauseMenuParent;

    private void Awake()
    {
        if (!isNotFirstTimePlaying)
        {
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            transform.parent = pauseMenuParent;
            gameObject.SetActive(false);
        }
    }
}
