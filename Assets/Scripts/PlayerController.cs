using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int numOfJumps;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] float staminaDrainRate;
    [SerializeField] int stamina;

    int jumpCount;
    Vector3 moveDirection;
    Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }

    void movement()
    {
        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }
        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDirection * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && jumpCount < numOfJumps)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
}
