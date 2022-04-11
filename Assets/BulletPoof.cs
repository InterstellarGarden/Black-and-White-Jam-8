using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoof : MonoBehaviour
{
    void Start()
    {
        if (TryGetComponent<ParticleSystem>(out ParticleSystem _particleSystem))
        {
            _particleSystem.Play();
            Destroy(gameObject, _particleSystem.main.duration + 1f);
        }
        else
        {
            Debug.LogError("No Particle System Attached To Poof");
        }
    }
}
