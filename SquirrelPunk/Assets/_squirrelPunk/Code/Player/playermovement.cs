using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    private Animator animator;

    [Header ("Stats")]
    public float speed = 6f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 30f;
    public float wallRunSpeed = 35f;
    public float wallKickForce = 5f;
    public float gravity = -9.81f;

    //coyote time and other buffers
    private float jumpBuffer;
    public float jumpBufferTimer = 0.1f;
    private float jumpCoolDown;
    private float jumpCoolDownCounter = .25f;
    private float groundBuffer;
    private float groundBufferTimer = 0.1f;
    private float wallJumpCD = 0f;
    public float wallJumpCDTimer = 0.1f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 lastAngleHit;

    [Header ("State Bools")]
    public Transform groundCheck;
    public bool checkForGround = true;
    public float groundDistance = 0.25f;
    public LayerMask groundMask;

    [SerializeField] private bool isGrounded;
    private bool jumped;
    [SerializeField] private bool wallRunning;
    public Vector3 velocity;
    Vector3 direction;

    public bool wallRunToggle;
    public bool canWallRun;
    public bool isSprinting;
    public bool doubleJumpOkay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip JumpSound;
    public AudioClip DoubleJumpSound;
    public AudioClip DeathSound;

    PlayerInput playerInput;

    public delegate void dying();
    public event dying died;

    // Update is called once per frame

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        lastAngleHit = Vector3.zero;
        canWallRun = true;
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (transform.parent == null)
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        jumpBuffer -= Time.deltaTime;
            Mathf.Clamp(jumpBuffer, 0, jumpBufferTimer);
        groundBuffer -= Time.deltaTime;
            Mathf.Clamp(groundBuffer, 0, groundBufferTimer);
        jumpCoolDown -= Time.deltaTime;
            Mathf.Clamp(jumpCoolDown, 0, jumpCoolDownCounter);
        wallJumpCD -= Time.deltaTime;
            Mathf.Clamp(wallJumpCD, 0, wallJumpCDTimer);

        if (checkForGround == true)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            try
            {
                Transform newGround = Physics.OverlapSphere(groundCheck.position, 0.5f, groundMask)[0].transform;
                transform.parent = newGround;
            }
            catch
            {

            }
            animator.SetBool("isGrounded", isGrounded);
        }

        if (isGrounded == true)
        {
            groundBuffer = groundBufferTimer;
            canWallRun = true;
            doubleJumpOkay = true;
            velocity.x = 0;
            velocity.z = 0;

            if (direction.magnitude == 0f)
            {
                SetAnimator(0);
            }
            else
            {
                if (isSprinting == false)
                    SetAnimator(1);
                else
                    SetAnimator(2);
            }
        }
        else
        {
            groundDistance = 0.25f;
            if (doubleJumpOkay == false)
                SetAnimator(7);
            else
                SetAnimator(6);

            //Wallrunning handler
            if (wallRunToggle == true)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, 1.5f, 9);

                if (cols.Length > 0 && wallJumpCD <= 0)
                {
                    gravity = 0;
                    velocity = Vector3.zero;
                    wallRunning = true;
                }
                else
                {
                    wallRunning = false;
                    ResetGravity();
                }
            }
            else
            {
                wallRunning = false;
                ResetGravity();
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Air DI
        if (direction.magnitude >= 0.1f && wallRunning == false)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //this is handling x & z axis movement
            if (isSprinting == false)
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * sprintSpeed * Time.deltaTime);
            }
        }
        else if (wallRunning == true)
        {
            animator.SetInteger("AnimState", (int)WallRunCheck());
            canWallRun = false;

            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = transform.forward;
            controller.Move(moveDir.normalized * wallRunSpeed * Time.deltaTime);

            if (jumpBuffer > 0 && jumpCoolDown < 0 && jumped == true)
            {
                wallRunning = false;
                ResetGravity();
                WallJump();
            }

            if (Mathf.Abs(controller.velocity.x) < 10 && Mathf.Abs(controller.velocity.z) < 10)
            {
                wallRunning = false;
                ResetGravity();
            }
        }
        if (jumpBuffer > 0 && groundBuffer > 0 && jumpCoolDown < 0 && jumped == true)
        {
            jumpCoolDown = jumpCoolDownCounter;
            Jump();
        }

        if (jumpBuffer > 0 && isGrounded == false && doubleJumpOkay == true && jumpCoolDown < 0 && jumped == true)
        {
            jumpCoolDown = jumpCoolDownCounter;
            DoubleJump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        direction = new Vector3(inputVec.x, 0, inputVec.y);
    }

    public void OnJump()
    {
        jumped = true;
        jumpBuffer = jumpBufferTimer;
    }

    public void OnParkour(InputValue value)
    {
        wallRunToggle = value.isPressed;

        if (value.isPressed == false)
        {
            ResetGravity();
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void Jump()
    {
        audioSource.PlayOneShot(JumpSound);
        transform.parent = null;
        SetAnimator(6);
        isGrounded = false;
        jumpBuffer = 0;
        groundBuffer = 0;
        checkForGround = false;
        StartCoroutine(DelayCheckForGround());
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    IEnumerator DelayCheckForGround()
    {
        yield return new WaitForSeconds(0.25f);
        checkForGround = true;
    }

    public void DoubleJump()
    {
        audioSource.PlayOneShot(DoubleJumpSound);
        SetAnimator(7);
        jumpBuffer = 0;
        doubleJumpOkay = false;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    public void WallJump()
    {
        SetAnimator(6);
        jumpBuffer = 0;
        wallJumpCD = wallJumpCDTimer;
        velocity.x = lastAngleHit.x * wallKickForce;
        velocity.z = lastAngleHit.z * wallKickForce;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    public void ResetGravity()
    {
        gravity = -33;
    }

    void SetAnimator(int index)
    {
        animator.SetInteger("AnimState", index);
    }

    float WallRunCheck()
    {
        if (ShootRayCast(transform.right))
        {
            return 5;
        }
        else if (ShootRayCast(-transform.right))
        {
            return 4f;
        }
        return 6f;
    }

    bool ShootRayCast(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 5f))
        {
            if (hit.collider != null)
            {
                return true;
            }
            else if (hit.collider == null)
            {
                return false;
            }
        }
        return false;
    }

    //subscribers: TimeTrial.cs
    public void Die()
    {
        Debug.Log("Die() called");
        CharacterController cc = this.GetComponent<CharacterController>();
        cc.enabled = false;

        audioSource.PlayOneShot(DeathSound);

        if (Game_Manager.instance != null)
            Game_Manager.instance.Respawn(this);

        if (died != null)
            died();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "deathZone")
        {
            Die();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hello");
    }
}
