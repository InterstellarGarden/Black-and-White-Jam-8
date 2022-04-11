using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBehaviour : MonoBehaviour
{
    public float maxCombo;
    public bool canInstantKill = false;


    [HideInInspector] public float currentCombo = 0;
    [SerializeField] float comboDecayRate;
    private Coroutine coroComboDecay;
    private bool isDecaying = false;

    //SOUND
    [SerializeField] private AudioClip comboReady;
    [Range(0, 1)] [SerializeField] private float comboReadySfxLocalMultiplier;

    private void Awake()
    {
        currentCombo = 0;
        canInstantKill = false;
    }
    void Start()
    {
        isDecaying = true;
        coroComboDecay = StartCoroutine(ComboDecay());
    }   

    public void TriggerAddCombo(float _delta)
    {
        StartCoroutine(DelayAddCombo(_delta));
    }

    IEnumerator DelayAddCombo(float _delta)
    {
        /////DELAY ADD COMBO TO PREVENT GLITCHES
        ///When killing an enemy results in max combo, adding this will make sure it does not instantly register into the RevolverBehaviour and cause mishaps
        ///such as instantly leading to Trigger_ResetDecay on the same frame.
        yield return new WaitForFixedUpdate();
        currentCombo += _delta;
        if (currentCombo >= maxCombo)
        {
            //STOP DECAY ON MAX COMBO
            TriggerStopDecay();

            //GUARANTEE NEXT SHOT INSTANT KILL HERE
            canInstantKill = true;
        }
    }

    public void TriggerRestartDecay()
    {
        //RESET COMBO
        currentCombo = 0;

        //RESET CONDITION
        canInstantKill = false;

        //RESET COROUTINE
        if (coroComboDecay != null)
            StopCoroutine(coroComboDecay);
        isDecaying = true;
        coroComboDecay = StartCoroutine(ComboDecay());
    }

    void TriggerStopDecay()
    {
        isDecaying = false;
        FindObjectOfType<SoundManager>().TriggerPlaySound(comboReady, comboReadySfxLocalMultiplier, false);
        StopCoroutine(coroComboDecay);
    }
    IEnumerator ComboDecay()
    {
        while (isDecaying)
        {
            currentCombo -= comboDecayRate * Time.deltaTime;

            if (currentCombo <= 0)
                currentCombo = 0;

            yield return new WaitForEndOfFrame();
        }

    }
}
