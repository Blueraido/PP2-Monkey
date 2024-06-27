using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementC : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] LayerMask isGround;
    [SerializeField] Transform orientation;
    [SerializeField] Camera miniMapCamera;

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
    [SerializeField] float stamina;

    //Used for input
    float horizontal;
    float vertical;
    float moveSpeed;
    float initialHeight;
    float miniMapOrigFOV;

    Vector3 moveDirection;
    RaycastHit slopeHit;
    bool isSprinting;

    float staminaOrig;
    Rigidbody rb;
    bool isGrounded = true;
    bool canJump = true;
    int timesJumped;
    movementSpeed state;
    Coroutine staminaRegen;

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
        staminaOrig = stamina;
        initialHeight = transform.localScale.y;
        miniMapOrigFOV = miniMapCamera.orthographicSize;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        UpdateUI();
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

        if (isSprinting && vertical == 1)
        {
            stamina -= 2 * Time.deltaTime;
            staminaCheck();
            UpdateUI();
            coroutineHandler();
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
            stamina--;
            staminaCheck();
            UpdateUI();
            coroutineHandler();
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
            miniMapCamera.fieldOfView = miniMapOrigFOV;

        }



    }

    private void speedHandler()
    {

        if(isGrounded && Input.GetButton("Crouch"))
        {
            state = movementSpeed.crouching;
            moveSpeed = crouchSpeed;
            isSprinting = false;
        }
        else if (isGrounded && Input.GetButton("Sprint") && stamina > 0)
        {
            state = movementSpeed.sprinting;
            moveSpeed = sprintSpeed;
            isSprinting = true; 
        }
        else if(isGrounded)
        {
            state = movementSpeed.walking;
            moveSpeed = walkSpeed;
            isSprinting = false;
        }
        else
        {
            state = movementSpeed.midAir;
            isSprinting = false;
        }
    }

    private void move()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        if(slope())
        {
            rb.AddForce(slopeDirection() * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);   
            }
        }

        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * jumpSpeedMultiplier, ForceMode.Force);
        }

        rb.useGravity = !slope();

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

    public float GetSpeed()
    {
        return walkSpeed;
    }
    public int GetJumps()
    {
        return numOfJumps;
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
    IEnumerator regenStamina()
    {
        yield return new WaitForSeconds(2f);
        while (stamina < staminaOrig)
        {
            stamina += 0.2f;
            staminaCheck();
            UpdateUI();
            yield return new WaitForSeconds(0.1f);
        }
    }
    void staminaCheck()
    {
        if (stamina < 0)
        {
            stamina = 0;
            isSprinting = false;
        }
        else if (stamina > staminaOrig)
        {
            stamina = staminaOrig;
        }
    }
    void coroutineHandler()
    {
        if (staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
        }
        staminaRegen = StartCoroutine(regenStamina());
    }

    public void AddStaminaMax(int toAdd)
    {
        staminaOrig += toAdd;
        UpdateUI();
    }

    public void AddStamina(int toAdd) // May or may not get used
    {
        if (toAdd + stamina <= staminaOrig)
        {
            stamina += staminaOrig;
        }
        else if (toAdd + stamina > staminaOrig)
        {
            stamina = staminaOrig;
        }

        UpdateUI();
    }

    public float GetStaminaMax()
    {
        return staminaOrig;
    }
    public float GetStamina()
    {
        return stamina;
    }

    public void UpdateUI()
    {
        GameManager.instance.playerStaminaBar.fillAmount = stamina / staminaOrig;
        GameManager.instance.playerStaminaValueText.text = ((int)stamina).ToString() + " / " + ((int)staminaOrig).ToString();
    }
}
