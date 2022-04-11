using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource thisSoundSource;
    [SerializeField] private AudioMixerGroup sfxGroup;
    private void Awake()
    {
        thisSoundSource = GetComponent<AudioSource>();
    }
    public void TriggerPlaySound(AudioClip _sound, float _localMultiplier, bool _randomise)
    {
        if (_randomise)
            thisSoundSource.pitch = Random.Range(0.75f, 1.25f);

        else thisSoundSource.pitch = 1;

        thisSoundSource.PlayOneShot(_sound, _localMultiplier);
    }

    public void RequestPlaySound(AudioSource _source, float _localMultiplier)
    {
        _source.pitch = Random.Range(0.75f, 1.25f);
        _source.outputAudioMixerGroup = sfxGroup;
        _source.PlayOneShot(_source.clip, _localMultiplier);
    }
}
