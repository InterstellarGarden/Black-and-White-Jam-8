using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBreak : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Breakable[] objs = GameObject.FindObjectsOfType<Breakable>();
            foreach(Breakable obj in objs)
            {
                obj.Break();
            }
        }
    }
}
