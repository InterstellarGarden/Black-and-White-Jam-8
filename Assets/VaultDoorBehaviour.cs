using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoorBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CarriageManager.playerHasTnt)
        {
            gameObject.GetComponentInParent<CarriageData>().UpdateCarriageState(false);
            CarriageManager.playerHasTnt = false;
        }
    }
}
