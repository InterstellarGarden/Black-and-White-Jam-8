using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoorBehaviour : MonoBehaviour
{
    public static bool isOpened = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CarriageManager.playerHasTnt)
        {
            FindObjectOfType<CarriageManager>().UpdateVaultOpened(true);
            gameObject.GetComponentInParent<CarriageData>().UpdateCarriageState(false);
            FindObjectOfType<CarriageManager>().UpdateTnt(false);
        }
    }
}
