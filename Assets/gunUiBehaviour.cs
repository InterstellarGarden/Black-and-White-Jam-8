using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gunUiBehaviour : MonoBehaviour
{
    private Image thisImage;
    public Sprite gunIdle, gunFire, gunCharged;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
        Idle();
    }
    public void Fire()
    {
        thisImage.sprite = gunFire;
        StartCoroutine(ReturnToIdle());
    }

    public void Idle()
    {
        if (FindObjectOfType<ComboBehaviour>().canInstantKill)
            thisImage.sprite = gunCharged;

        else thisImage.sprite = gunIdle;
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1/(2*FindObjectOfType<RevolverBehaviour>().fireRate));
        Idle();
    }
}
