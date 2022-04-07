using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    //MOUSE CONTROL
    public float lookSpeed, lookXLimit;
    float rotationX = 0;

    //MOVEMENT PROPERTIES
    CharacterController characterController;
    public float walkingSpeed = 5;
    public float runningSpeed, jumpForce;
    public bool canMove = true;
    Vector3 moveDirection = Vector3.zero;
    public float gravity = 20f;

    public float maxStamina;
    [HideInInspector] public float currentStamina;
    bool staminaCoroActivated = false;
    bool isRunning = false;


    //CROUCHING
    public float normalHeight = 2,crouchingHeight, crouchingYCenter;
    [HideInInspector] public bool isCrouching;

    //POWERUPS
    bool powerUpSpeedUp, powerUpFastFire = false;

    //DEATH
    public int maxHealth;
    private int health;
    [SerializeField] private int lowHealthThreshold = 2;
    public bool isLowHealth = false;

    //PRIVATE ESSENTIALS
    Camera playerCamera;

    private void Awake()
    {
        //MOVEMENT
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        playerCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        canMove = true;
        isRunning = false;

        //STAMINA
        staminaCoroActivated = true;

        //HEALTH
        health = maxHealth;
        isLowHealth = false;
    }
    void Start()
    {      
    }

    void Update()
    {
        //Debug code - to be removed at end
        if (Input.GetKeyDown(KeyCode.O))
            TriggerTakeDamage(1);

        if (GameManager.playerIsDead)
            return;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run - stop running when out of stamina
        isRunning = Input.GetKey(KeyCode.LeftShift);

        //Press Left Control to crouch 
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        switch (isCrouching)
        {
            case true:
                //interrupts any attempt to run
                isRunning = false;

                characterController.height = crouchingHeight;
                characterController.center = new Vector3(0, crouchingYCenter, 0);
                break;

            case false:
                characterController.height = normalHeight;
                characterController.center = new Vector3(0, 0, 0);
                break;
        }

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }        
    }
    public void TriggerTakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
            GameManager.instance.GameOver();

        else if (health <= lowHealthThreshold)
            isLowHealth = true;
    }
    void TriggerRecoverDamage(int _delta)
    {
        health += _delta;

        if (health > lowHealthThreshold)
            isLowHealth = false;

        if (health > maxHealth)
            health = maxHealth;
    }
    public void TriggerPowerUp(int _choice)
    {
        switch (_choice)
        {
            case (int)TemporaryPickUp.types.tnt:
                Debug.Log("obtained tnt");
                CarriageManager.playerHasTnt = true;
                break;
            
            case (int)TemporaryPickUp.types.health:
                Debug.Log("obtained health");
                TriggerRecoverDamage(2); 
                break;
        }
    }
    public void GameOver()
    {
        StopAllCoroutines();
    }    
}
