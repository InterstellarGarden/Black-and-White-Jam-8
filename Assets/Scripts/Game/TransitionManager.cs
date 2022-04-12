using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;    
    private void Awake()
    {
        transitionAnimator = GetComponent<Animator>();
    }        
    void TriggerLoadScene(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }

    public void LoadScene(int _buildIndex)
    {
        StartCoroutine(CoroStartGame(_buildIndex));
    }
    private IEnumerator CoroStartGame(int _buildIndex)
    {
        //Rudimentary system
        //To upgrade: Can be built better using async scene loading
        transitionAnimator.Play("transition_fadetoblack");

        //Let transition animation fully run
        yield return new WaitForSeconds(2);

        //To upgrade: If async scene is fully loaded, set it as active scene
        TriggerLoadScene(_buildIndex);
    }    
    public void GameOver()
    {
        //Run animation here for GameOver
    }
}
