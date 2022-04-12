using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCinematic : MonoBehaviour
{
    public float speed = 1;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
    }

    

}
