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
    [SerializeField] int sprintCost; //Stamina cost for Sprinting
    [SerializeField] int jumpCost; //Stamina cost for jumping
    [SerializeField] int staminaRegenRate; //How many stamina points are regenerated per tick
    [SerializeField] float staminaRegenDelay; //Delay before stamina starts regenerating
    [SerializeField] int stamina;
    int staminaOrig;
    WaitForSeconds regenSpeed = new WaitForSeconds(1); //Used to set how fast stamina regenerates

    [SerializeField] int shootDamage;
    [SerializeField] float shootSpeed;
    [SerializeField] int shootDistance;

    [SerializeField] GameObject projectile;
    bool isShooting;
    int HpOriginal;
    int jumpCount;
    Vector3 moveDirection;
    Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        staminaOrig = stamina;
        HpOriginal = HP;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }
        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDirection * speed * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && jumpCount < numOfJumps && stamina > 0)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            useStamina(jumpCost);
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && stamina > 0)
        {
            speed *= sprintMod;
            useStamina(sprintCost);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, Camera.main.transform.position + new Vector3(0, 0, 1), Camera.main.transform.rotation);
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdateUI();
        if (HP <= 0)
        {
            GameManager.instance.updateLose();
        }
    }

    void useStamina(int amount)
    {
        if (staminaOrig - stamina > 0)
        {
            stamina -= amount;
            //Code for stamina bar when I can access the game manager
            StartCoroutine(regenStamina());
        }
    }

    IEnumerator regenStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);

        while (stamina < staminaOrig)
        {
            stamina += staminaRegenRate;
            //Code for stamina bar when I can access the game manager
            yield return regenSpeed;
        }
    }
    void UpdateUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HpOriginal;
        GameManager.instance.playerStaminaBar.fillAmount = stamina / staminaOrig;
    }
}

