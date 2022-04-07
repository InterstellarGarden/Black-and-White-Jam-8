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

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", .5f); // Gets the float value of musicVolume, or uses .5f if it isn't found.
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", .5f); // Gets the float value of SFXVolume, or uses .5f if it isn't found.
    }

    public void SetMusicVolume(float musicVol)
    {
        musicMixer.SetFloat("musicVolume", Mathf.Log10(musicVol) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicVol); // Sets the value of SliderVolume to the music volume value.

        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sfxVol)
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVol); // Sets the value of SliderVolume to the sound effect volume value.
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVol) * 20);

        PlayerPrefs.Save();
    }
}