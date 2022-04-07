using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReggieBehaviour : EnemyBehaviour
{
    [Range(1, 10)][SerializeField]
    private float movementSpeed;
    [Range(0, 180)][SerializeField]
    private int movementDirection;
    [Range(1, 10)][SerializeField]
    private int movementMagnitude;
    private float displacementIncrementer;
private Transform transform;
    private Vector3 homePosition;
    public void Awake(){
        this.transform = GetComponent<Transform>();
        this.homePosition = this.transform.position;
        this.displacementIncrementer = 0f;
    }

    public void Update(){
        Vector3 displacement = Vector3.left * Mathf.Sin(this.displacementIncrementer) * this.movementMagnitude;
        displacement = Quaternion.Euler(0, this.movementDirection, 0) * displacement;

        this.transform.position = this.homePosition + displacement;
        this.displacementIncrementer += (this.movementSpeed / 100);

        // prevent overflows
        if(this.displacementIncrementer >= 360){
            this.displacementIncrementer -= 360;
        }
    }
}
