using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementC : MonoBehaviour, IDamage
{
    [SerializeField] float jumpSpeed;
    [SerializeField] int numOfJumps;
    [SerializeField] float jumpSpeedMultiplier;
    [SerializeField] float moveSpeed;
    [SerializeField] float height;
    [SerializeField] float drag;
    [SerializeField] LayerMask isGround;
    [SerializeField] Transform orientation;
    Vector3 moveDirection;

    //Used for input
    float horizontal;
    float vertical;

    Rigidbody rb;
    bool isGrounded = true;
    bool canJump = true;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for ground below player using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, isGround);
        dirInput();
        if(isGrounded)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0;
        }

        speedCheck();
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

        if(Input.GetButtonDown("Jump") && canJump && isGrounded)
        {
            canJump = false;
            jumpProcess();
            Invoke(nameof(jumpRefresh), 0.25f);
        }
    }

    private void move()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
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
        moveSpeed += toAdd;
    }

    public void AddJumps(int toAdd)
    {
        numOfJumps += toAdd;
    }
}
