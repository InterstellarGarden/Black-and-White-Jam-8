using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReggieBehaviour : EnemyBehaviour
{
    [Range(1, 10)][SerializeField]
    private float movementSpeed = 1f;
    [Range(0, 180)][SerializeField]
    private int movementDirection = 0;
    [Range(1, 10)][SerializeField]
    private int movementMagnitude = 1;
    private Transform transform;
    private Vector3 homePosition;
    public void Awake(){
        this.transform = GetComponent<Transform>();
        this.homePosition = this.transform.position;
    }

    public void Update(){
        Vector3 displacement = Vector3.left * Mathf.Sin(Time.time * this.movementSpeed) * this.movementMagnitude;
        displacement = Quaternion.Euler(0, this.movementDirection, 0) * displacement;

        this.transform.position = this.homePosition + displacement;
    }
}
