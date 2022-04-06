using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PickUP : MonoBehaviour
{
    private Animator thisAnimator;

    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPickUp();
    }
    protected virtual void OnPickUp()
    {
        Destroy(gameObject);
    }
}
