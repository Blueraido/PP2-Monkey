using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementC : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] LayerMask isGround;
    [SerializeField] Transform orientation;

    [Header("Jumping stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] int numOfJumps;
    [SerializeField] float jumpSpeedMultiplier;

    [Header("Speed and physics stats")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchHeight;
    [SerializeField] float height;
    [SerializeField] float drag;
    [SerializeField] float maxSlopeAngle;

    //Used for input
    float horizontal;
    float vertical;
    float moveSpeed;
    float initialHeight;

    Vector3 moveDirection;
    RaycastHit slopeHit;

    Rigidbody rb;
    bool isGrounded = true;
    bool canJump = true;
    int timesJumped;
    movementSpeed state;

    enum movementSpeed
    {
        walking,
        sprinting,
        crouching,
        midAir
    }

    // Start is called before the first frame update
    void Start()
    {
        initialHeight = transform.localScale.y;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for ground below player using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, isGround);
        dirInput();
        speedCheck();
        speedHandler();
        if(isGrounded)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0;
        }


    }
    public void takeDamage(float damage)
    {
        PlayerController.instance.takeDamage(damage);
    }

    private void FixedUpdate()
    {
        move();
    }

    //Used to get keyboard input
    private void dirInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(isGrounded)
        {
            timesJumped = 0;
        }

        //Jump function
        if(Input.GetButtonDown("Jump") && canJump && timesJumped < numOfJumps)
        {
            canJump = false;
            timesJumped++;
            jumpProcess();
            Invoke(nameof(jumpRefresh), 0.25f);
        }

        //Crouch function
        if(Input.GetButtonDown("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, initialHeight, transform.localScale.z);
        }
    }

    private void speedHandler()
    {

        if(isGrounded && Input.GetButton("Crouch"))
        {
            state = movementSpeed.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (isGrounded && Input.GetButton("Sprint"))
        {
            state = movementSpeed.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(isGrounded)
        {
            state = movementSpeed.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = movementSpeed.midAir;
        }
    }

    private void move()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        if(slope())
        {
            rb.AddForce(slopeDirection() * moveSpeed * 20f, ForceMode.Force);
        }

        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * jumpSpeedMultiplier, ForceMode.Force);
        }

    }

    private void speedCheck()
    {
        //Checks to see if player is moving faster than movement speed
        Vector3 rbVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(rbVel.magnitude > moveSpeed)
        {
            Vector3 maxVel = rbVel.normalized * moveSpeed;
            rb.velocity = new Vector3(maxVel.x, rb.velocity.y, maxVel.z);
        }
    }
    private void jumpProcess()
    {
        //Resets y velocity to ensure always jumping the same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }
    private void jumpRefresh()
    {
        canJump = true;
    }

    public void AddSpeed(int toAdd)
    {
        sprintSpeed += toAdd;
        walkSpeed += toAdd;
        crouchSpeed += toAdd;  
    }

    public void AddJumps(int toAdd)
    {
        numOfJumps += toAdd;
    }

    private bool slope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    
    private Vector3 slopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
}
