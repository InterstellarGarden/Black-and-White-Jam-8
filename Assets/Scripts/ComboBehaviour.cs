using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBehaviour : MonoBehaviour
{
    public float maxCombo;
    public bool canInstantKill = false;


    [HideInInspector] public float currentCombo = 0;
    [SerializeField] float comboDecayRate;
    private Coroutine coro_ComboDecay;
    private bool isDecaying = false;
    

    private void Awake()
    {
        currentCombo = 0;
        canInstantKill = false;
    }
    void Start()
    {
        isDecaying = true;
        coro_ComboDecay = StartCoroutine(ComboDecay());
    }   

    public void Trigger_AddCombo(float _delta)
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
            Trigger_StopDecay();

            //GUARANTEE NEXT SHOT INSTANT KILL HERE
            canInstantKill = true;
        }
    }

    public void Trigger_RestartDecay()
    {
        //RESET COMBO
        currentCombo = 0;

        //RESET CONDITION
        canInstantKill = false;

        //RESET COROUTINE
        if (coro_ComboDecay != null)
            StopCoroutine(coro_ComboDecay);
        isDecaying = true;
        coro_ComboDecay = StartCoroutine(ComboDecay());
    }

    void Trigger_StopDecay()
    {
        isDecaying = false;
        StopCoroutine(coro_ComboDecay);
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
