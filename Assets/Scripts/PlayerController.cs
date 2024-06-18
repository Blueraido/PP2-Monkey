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
    [SerializeField] float stamina;
    WaitForSeconds staminaRegenSpeed = new WaitForSeconds(0.1f); //Created to prevent using too much memeory, used to set how fast stamina regens
    float staminaOrig;
    bool isSprinting;
    float origSpeed;
    Coroutine staminaRegen;

    [SerializeField] float shootSpeed;


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
        origSpeed = speed;
        HpOriginal = HP;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            speed = origSpeed;
        }
        if(isSprinting && moveDirection.magnitude != 0)
        {
            stamina -= sprintCost * Time.deltaTime;
            staminaCheck();
            UpdateUI();
            coroutineHandler();
        }
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
        if (Input.GetButtonDown("Jump") && jumpCount < numOfJumps && stamina >= jumpCost)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            stamina -= jumpCost;
            staminaCheck();
            UpdateUI();
            coroutineHandler();
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    IEnumerator regenStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);
        while(stamina < staminaOrig)
        {
            stamina += staminaRegenRate / 10f;
            staminaCheck();
            UpdateUI();
            yield return staminaRegenSpeed;
        }
    }
    void staminaCheck()
    {
        if(stamina < 0)
        {
            stamina = 0;
            isSprinting = false;
            speed = origSpeed;
        }
        else if(stamina > staminaOrig)
        {
            stamina = staminaOrig;
        }
    }
    void coroutineHandler()
    {
        if(staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
        }
        staminaRegen = StartCoroutine(regenStamina());
    }
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, Camera.main.transform.position + new Vector3(0, 1, 1), Camera.main.transform.rotation);
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
    void UpdateUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HpOriginal;
        GameManager.instance.playerStaminaBar.fillAmount = (float)stamina / staminaOrig;
        GameManager.instance.playerHealthValueText.text = HP.ToString() + " / " + HpOriginal.ToString();
        GameManager.instance.playerStaminaValueText.text = stamina.ToString() + " / " + staminaOrig.ToString();
    }
}

