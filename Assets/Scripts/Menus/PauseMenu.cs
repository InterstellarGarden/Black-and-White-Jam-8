#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private void Awake()
    { Resume(); } // Makes sure that the level isn't started in a paused state.

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            { Resume(); }
            else
            { Pause(); }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
    }
}
