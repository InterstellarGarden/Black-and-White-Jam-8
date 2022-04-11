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
    CapsuleCollider triggerCollider;
    public float walkingSpeed = 5;
    public float runningSpeed, jumpForce;
    public bool canMove = true;
    Vector3 moveDirection = Vector3.zero;
    Vector3 storedMoveDirection;
    public float gravity = 20f;
    public float airControlMultiplier = 0.2f;

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
    [HideInInspector] public int health;
    [SerializeField] private int lowHealthThreshold = 2;
    public bool isLowHealth = false;

    //PRIVATE ESSENTIALS
    Camera playerCamera;

    //SOUND
    private SoundManager thisSoundManager;
    [SerializeField] private AudioClip hurt, dead, pickUpHealth, pickUpTnt;

    private void Awake()
    {
        //MOVEMENT
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        triggerCollider = GetComponent<CapsuleCollider>();
        playerCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        canMove = true;
        isRunning = false;

        //STAMINA
        staminaCoroActivated = true;

        //HEALTH
        health = maxHealth;
        isLowHealth = false;

        thisSoundManager = FindObjectOfType<SoundManager>();
    }
    void Start()
    {      
    }

    void Update()
    {
        if (GameManager.playerIsDead)
            return;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run - stop running when out of stamina
        isRunning = Input.GetKey(KeyCode.LeftShift);

        //Press Left Control to crouch 
        isCrouching = Input.GetKey(KeyCode.C);
        switch (isCrouching)
        {
            case true:
                //interrupts any attempt to run
                isRunning = false;

                triggerCollider.height = characterController.height = crouchingHeight;
                triggerCollider.center = characterController.center = new Vector3(0, crouchingYCenter, 0);
                break;

            case false:
                triggerCollider.height = characterController.height = normalHeight;
                triggerCollider.center = characterController.center = new Vector3(0, 0, 0);
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

        storedMoveDirection = moveDirection;
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        // Player and Camera rotation
        if (!PauseMenu.GameIsPaused)
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
        {
            if (!GameManager.playerIsDead)
            {
                if (dead != null)
                    thisSoundManager.TriggerPlaySound(dead, 1, true);

                GameManager.instance.GameOver();
            }
            return;
        }

        else if (health <= lowHealthThreshold)
        {
            isLowHealth = true;
        }

        if (hurt != null)
            thisSoundManager.TriggerPlaySound(hurt, 1, true);
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
                FindObjectOfType<CarriageManager>().UpdateTnt(true);

                if (pickUpTnt != null)
                    thisSoundManager.TriggerPlaySound(pickUpTnt, 1, false);
                break;
            
            case (int)TemporaryPickUp.types.health:
                Debug.Log("obtained health");
                TriggerRecoverDamage(2);

                if (pickUpHealth != null)
                    thisSoundManager.TriggerPlaySound(pickUpHealth, 1, false);
                break;
        }
    }
    public void GameOver()
    {
        StopAllCoroutines();
    }

    
}
