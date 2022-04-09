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
    private Vector3 homePosition;

    protected override void Awake()
    {
        base.Awake();
        this.homePosition = transform.position;

    }
    public override void TriggerTakeDamage(int _bulletType)
    {
        base.TriggerTakeDamage(_bulletType);
    }
    
    public void Update(){
        Vector3 displacement = Vector3.left * Mathf.Sin(Time.time * this.movementSpeed) * this.movementMagnitude;
        displacement = Quaternion.Euler(0, this.movementDirection, 0) * displacement;

        transform.position = this.homePosition + displacement;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //Add gravity

    }
}
