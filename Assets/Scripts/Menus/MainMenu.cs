#region 'Using' information.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

public class MainMenu : MonoBehaviour
{
    /*
        Click on the buttons, then add one of the two methods below via the 'OnClicked' part that handles events in the inspector.         
    */

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!"); // Prints 'QUIT' if testing this in the editor.
        Application.Quit(); // Closes the game.
    }

    // Making the settings & options menu appear is all done in the inspector.
}