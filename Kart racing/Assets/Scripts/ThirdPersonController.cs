﻿

using UnityEngine;
using UnityEngine.InputSystem;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/


/// <summary>
/// Main script for third-person movement of the character in the game.
/// Make sure that the object that will receive this script (the player) 
/// has the Player tag and the Character Controller component.
/// </summary>
public class ThirdPersonController : MonoBehaviour
{

    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Player states
    public bool isJumping = false;
    //bool isSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputAttack;
    bool inputCrouch;
    bool inputSprint;
    public PlayerInput playerInput;
    public Player player;
    public Animator animator;
    public CharacterController cc;
    Ability specialAbility;
    public EnemyManager enemyManager;
    public float chargerTime;
    Pickups pick;
    public AudioSource runningS;
    
    //public bool isAnimating;
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        player.playerGotMolested += FallStart;
        // Message informing the user that they forgot to add an animator
        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");
    }
    private void Start()
    {
        specialAbility = player.specialPower;
        chargerTime = specialAbility.rechargeTime;
        pick = player.GetComponent<Pickups>();
    }
  
    float animationValue;
    // Update is only being used here to identify keys and trigger animations


    // Declare a variable to store the previous button state
    private bool wasButtonPressed = false;

    void Update()
    {
        if (layingDown == true || animator == null)
            return;

        // Input checkers
        inputHorizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;//Input.GetAxis("Horizontal");
        inputVertical = playerInput.actions["Move"].ReadValue<Vector2>().y;//Input.GetAxis("Vertical");
        inputAttack = playerInput.actions["Shoot"].ReadValue<float>()>0.1f;//Input.GetAxis("Jump") == 1f;
       // inputJump = playerInput.actions["Shoot"].ReadValue<float>()>0.1f;//Input.GetAxis("Jump") == 1f;
    //    inputSprint = playerInput.actions["Sprint"].ReadValue<float>() > 0.1f;//Input.GetAxis("Fire3") == 1f;
        // Unfortunately GetAxis does not work with GetKeyDown, so inputs must be taken individually
        //inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        // Check if you pressed the crouch input key and change the player's state
        if ( inputCrouch )
            isCrouching = !isCrouching;
        if (cc.velocity.magnitude>1)
            runningS.UnPause();
        else runningS.Pause();
        // Run and Crouch animation
        // If dont have animator component, this block wont run
        if ( cc.isGrounded )
        {
            // Crouch
            // Note: The crouch animation does not shrink the character's collider
            ////////// animator.SetBool("crouch", isCrouching);
            //            animator.SetBool("Shoot", inputAttack); //for shooting from button


            //MyChange
            // Read the current button value
            float buttonValue = playerInput.actions["Shoot2"].ReadValue<float>();
            bool isButtonPressed = buttonValue > 0.3f;

            // Check for the transition from not pressed to pressed
            if (isButtonPressed && !wasButtonPressed)
            {
                // Call the DeployMines method only once per button press
                pick.DeployMines();
            }

            // Update the previous button state
            wasButtonPressed = isButtonPressed;

            //////if(playerInput.actions["Shoot2"].ReadValue<float>() > 0.3f) pick.DeployMines();


            if (inputAttack && player.power.isActive)
            {
                player.isAnimatingPower = true;
                player.InitiateAttack();
            }
            else if(!player.power.isActive && chargerTime > 0)
            {
                chargerTime -= Time.deltaTime;
            }
            else if (!player.power.isActive)
            {
                //////player.power.isActive = true;

                if (UIManager.Instance.PowerBtn.fillAmount >= 0.99f)
                {
                    player.power.isActive = true;
                    UIManager.Instance.EnableDisablePowerButton(true);
                }

                chargerTime = specialAbility.rechargeTime;
            }
            //                      
            // Run
            float minimumSpeed = 0.9f;
            if (!player.isAnimatingPower) animator.SetBool("run", cc.velocity.magnitude > minimumSpeed );
            animator.SetFloat("speed", cc.velocity.magnitude);
            // Sprint
      //      isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
       //     if (!player.isAnimatingPower) animator.SetBool("sprint", isSprinting );

        }

        // Jump animation
        if (!player.isAnimatingPower) animator.SetBool("air", cc.isGrounded == false );

        // Handle can jump or not
        if ( inputJump && cc.isGrounded )
        {
            isJumping = true;
            // Disable crounching when jumping
            //isCrouching = false; 
        }

       // HeadHittingDetect();

    }
    public void ChangeAttackAnimationValue(float val)
    {
        animationValue=val;
    }

    // With the inputs and animations defined, FixedUpdate is responsible for applying movements and actions to the player
    private void FixedUpdate()
    {
        if (layingDown == true)
            return;
        // Sprinting velocity boost or crounching desacelerate
        float velocityAdittion = 0;
   //     if ( isSprinting )
      //      velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion =  - (velocity * 0.50f); // -50% velocity

        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        // Jump handler
        if ( isJumping )
        {
            // Apply inertia and smoothness when climbing the jump
            // It is not necessary when descending, as gravity itself will gradually pulls
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;
            //print(directionY+"Flying"+jumpForce);
            // Jump timer
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                player.isAnimatingPower = false;
                animator.SetBool("Flight", false);
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;

        
        // --- Character rotation --- 

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // --- End rotation ---

        
        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 moviment = verticalDirection + horizontalDirection;
        cc.Move( moviment );

    }

    public bool layingDown=false;
    public void FallFinish()
    {
        layingDown = false;
        player.canPlayerPick = true;
    }
    public void FallStart(Vector3 moviment)
    {
        layingDown = true;
        player.isAnimatingPower = false;
        inputHorizontal = inputVertical = 0;
        animator.SetTrigger("fall");
        Invoke(nameof(FallFinish),3);
    }

    //This function makes the character end his jump if he hits his head on something
    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        // Uncomment this line to see the Ray drawed in your characters head
        // Debug.DrawRay(ccCenter, Vector3.up * headHeight, Color.red);

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
