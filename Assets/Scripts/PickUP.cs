using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUP : MonoBehaviour
{
    
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
