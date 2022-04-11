#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
#endregion

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicMixer;   // The mixer that controls music volume.
    public AudioMixer SFXMixer;     // The mixer that controls sound effect volume.

    public Slider musicSlider; // The slider that controls music volume.
    public Slider SFXSlider; // The slider that controls sound effect volume.

    public Slider mouseSensSlider;
    public static float mouseSens;

    public GameObject parent;
    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", .5f); // Gets the float value of musicVolume, or uses .5f if it isn't found.
        musicMixer.SetFloat("musicVolume", musicSlider.value);

        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", .5f); // Gets the float value of SFXVolume, or uses .5f if it isn't found.
        SFXMixer.SetFloat("SFXVolume", SFXSlider.value);

        //Set mouse settings
        mouseSensSlider.value = PlayerPrefs.GetFloat("mouseSens", .5f);
        mouseSens = PlayerPrefs.GetFloat("mouseSens", .5f);

        //Disable self after initialising settings
        if (parent != null)
            transform.SetParent(parent.transform);

        gameObject.SetActive(false);
    }
 

    public void SetMusicVolume(float musicVol)
    {        
        PlayerPrefs.SetFloat("musicVolume", musicVol); // Sets the value of SliderVolume to the music volume value.
        musicMixer.SetFloat("musicVolume", Mathf.Log10(musicVol) * 20);

        if (musicVol <= 0)
            musicMixer.SetFloat("musicVolume", 0); //Avoid mathematical errors with Mathf.Log10(0);

        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sfxVol)
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVol); // Sets the value of SliderVolume to the sound effect volume value.
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVol) * 20);

        if (sfxVol <= 0)
            SFXMixer.SetFloat("SFXVolume", 0);

        PlayerPrefs.Save();
    }

    public void SetMouseSens(float _sens)
    {
        PlayerPrefs.SetFloat("mouseSens", _sens);
        mouseSens = _sens;


        PlayerPrefs.Save();
    }
}