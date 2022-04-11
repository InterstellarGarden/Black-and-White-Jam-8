using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoorBehaviour : MonoBehaviour
{
    public static bool isOpened = false;
    public AudioClip tntUsed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CarriageManager.playerHasTnt)
        {
            FindObjectOfType<CarriageManager>().UpdateVaultOpened(true);
            gameObject.GetComponentInParent<CarriageData>().UpdateCarriageState(false);
            FindObjectOfType<CarriageManager>().UpdateTnt(false);

            FindObjectOfType<SoundManager>().TriggerPlaySound(tntUsed,1f,false);
        }
    }
}
