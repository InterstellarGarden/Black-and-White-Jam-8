using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutscenePanel
    {
        public Sprite image;
    }

    public CutscenePanel[] panels;

    public GameObject nextButton;
    public GameObject skipButton;
    public Image sprite;

    public int buildIndexToLoad;

    public float showNextButtonDelay = 1.0f;
    float countDown = -10f;

    int current = 0;

    void Start()
    {
        Next();
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown >= 0f)
        {
            countDown -= Time.deltaTime;

            if (countDown < 0f)
            {
                nextButton.SetActive(true);
            }
        }
    }

    public void Skip()
    {
        FindObjectOfType<TransitionManager>().LoadScene(buildIndexToLoad);
    }

    public void Next()
    {
        int i = current++;
        if (i >= panels.Length)
        {
            Skip();
            return;
        }

        var panel = panels[i];

        countDown = Mathf.Max(0, showNextButtonDelay);
        nextButton.SetActive(false);
        sprite.sprite = panel.image;
    }
}