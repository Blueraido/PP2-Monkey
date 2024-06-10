using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int numOfJumps;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] float staminaDrainRate;
    [SerializeField] int stamina;

    [SerializeField] int shootDamage;
    [SerializeField] float shootSpeed;
    [SerializeField] int shootDistance;

    [SerializeField]  GameObject projectile;
    bool isShooting;
    int HpOriginal;
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
        HpOriginal = HP;
        movement();
        sprint();
        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());
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
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, Camera.main.transform.position, Camera.main.transform.rotation);
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP-=amount;
        //updatePlayerUI();
        if(HP < 0)
        {
            //gameManager.instance.youLose();
        }
    }
}
