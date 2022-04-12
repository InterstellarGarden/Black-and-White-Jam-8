using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    private Transform oriPos,camCrouchPos;

    Vector3 bulletSpawnOri;
    private CharacterBehaviour thisPlayer;
    CinemachineVirtualCamera activeCam;
    Coroutine cameraShaking;

    private void Awake()
    {
        thisPlayer = FindObjectOfType<CharacterBehaviour>();
        activeCam = GetComponentInChildren<CinemachineVirtualCamera>();

    }
    void Update()
    {
        switch (thisPlayer.isCrouching)
        {
            case true:
                transform.position = camCrouchPos.position;
                break;

            case false:
                transform.position = oriPos.position;
                break;
        }
    }

    public void TriggerCameraShake(float _amplitude, float _frequency, float _duration)
    {
        
        if (cameraShaking != null)
        {
            activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
            StopCoroutine(cameraShaking);
        }

        activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _amplitude;
        activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = _frequency;

        cameraShaking = StartCoroutine(DecayCameraShake(_amplitude, _frequency, _duration));

    }
    IEnumerator DecayCameraShake(float _amplitude, float _frequency, float _duration)
    {
        float _amplitudeDelta = _amplitude / (_duration * 60);
        float _frequencyDelta = _frequency / (_duration * 60);
        for (int t = 0; t < _duration * 60; t++)
        {
            activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain -= _amplitudeDelta;
            activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain -= _frequencyDelta;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
}
