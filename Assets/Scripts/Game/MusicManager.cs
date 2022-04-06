using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;

    public static float musicVolume;
    public float timeToChangeMusicInSecs;
    public List<AudioSource> musicSources;

    public enum music
    {
        calm = 0,
        combat = 1
    }

    bool horizontalLayering;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        musicVolume = 1;
    }
    private void Start()
    {
        musicSources[(int)music.calm].volume = musicVolume;
        musicSources[(int)music.combat].volume = 0;

        foreach (AudioSource _source in musicSources)
            _source.Play();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            RequestHorizontalLayer(musicSources[(int)music.combat]);
    }
    public void RequestHorizontalLayer(AudioSource _desired)
    {
        StartCoroutine(HorizontalLayering(_desired));
    }

    IEnumerator HorizontalLayering(AudioSource _desired)
    {
        //GUARD
        if (!horizontalLayering)
        {
            horizontalLayering = true;
            for (int t = 0; t <= 60 * timeToChangeMusicInSecs; t++)
            {
                //LOWER VOLUME OF EVERYTHING ELSE
                foreach (AudioSource _source in musicSources)
                    if (_source != _desired)
                    {
                        float _deltaToZero = musicVolume*(_source.volume / (60 * timeToChangeMusicInSecs));
                        _source.volume -= _deltaToZero;
                    }

                //RAISE VOLUME OF DESIRED
                float _delta = musicVolume * (1 / (60 * timeToChangeMusicInSecs));
                _desired.volume += _delta;

                yield return new WaitForFixedUpdate();
            }

            //SET VOLUME AFTER LOOP FOR INCONSISTENCIES
            foreach (AudioSource _source in musicSources)
                if (_source != _desired)
                    _source.volume = 0;
            _desired.volume = musicVolume;

            horizontalLayering = false;         
        }
    }
}
