using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;

public class GameManager : MonoBehaviour
{
    // Obtain player references
    public static GameManager instance;
    public GameObject player;
    public GameObject playerMovement;

    // Player script reference
    public PlayerController playerScript;
    public PlayerMovementC playerMovementScript;

    // Projectile reference
    public Projectile projectileScript;

    // Hud element processing
    [SerializeField] public Image playerHPBar;
    [SerializeField] public Image playerStaminaBar;
    [SerializeField] public Image playerExpBar;

    //Health and Stamina Bar text processing
    [SerializeField] public TMP_Text playerHealthValueText;
    [SerializeField] public TMP_Text playerStaminaValueText;

    // Exp and level text processing
    [SerializeField] public TMP_Text playerExpValueText;
    [SerializeField] public TMP_Text playerLevelValueText;

    // Pause menu processing
    public bool isPaused;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuLevelUp; // Managed by LevelManager.cs
    [SerializeField] GameObject menuStats;

    // Objective text ui processing
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text splatCountText;
    public int enemyCount;
    int splatCount;

    // Stats being referenced in menuStats
    [SerializeField] TMP_Text statDamageCurrGun;
    [SerializeField] TMP_Text statDamageMult;
    [SerializeField] TMP_Text statTotal;
    [SerializeField] TMP_Text statSpeed;
    [SerializeField] TMP_Text statNumJumps;

    [SerializeField] public TMP_Text ammoCurrent;
    [SerializeField] public TMP_Text ammoMax;
    [SerializeField] public TMP_Text weaponName;


    // Toggle Visible UI Elements
    [SerializeField] Image reticle;
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerMovement = GameObject.FindWithTag("PlayerMovement");
        playerMovementScript = playerMovement.GetComponent<PlayerMovementC>();
    }

    void Update()
    {
        if (!ExpManager.instance.enabled)
        {
            ExpManager.instance.enabled = true; // I have no earthly idea why this is necessary but so be it
            ExpManager.instance.UpdateUI();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null) // Pauses the game when cancel is pressed and current active menu is null
                menuProcess(menuPause); // Makes current menu the pause menu

            else if (menuActive == menuPause) // Allows only pause menu to be escaped out of
                menuProcess(); // Unpause is done by passing in nothing

            else { } // Do nothing if other assumed menu is on screen (only pause should be esc out of)
        }

        if (Input.GetButtonDown("Tab"))
        {
            DisplayStats();
            Debug.Log("Tab being held...");
            menuStats.SetActive(true);

        }
        else if (Input.GetButtonUp("Tab"))
        {
            Debug.Log("...you let go.");
            menuStats.SetActive(false);
        }
    }

    public void menuProcess(GameObject menu = null)
    {
        if (menu != null)
        {
            if (menuActive != null)
                return;
            togglePause(); // Pauses when isPaused = false, unpauses if the opposite is true
            menuActive = menu; // Sets the active menu to parsed menu
            menuActive.SetActive(isPaused);
        }

        else if (menu == null) // Order of operation changes when unpausing
        {
            togglePause();
            menuActive.SetActive(isPaused);
            menuActive = menu;
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused; // Inverts pause state here

        if (isPaused) // Pauses scene
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            reticle.enabled = false;
        }

        else if (!isPaused) // Unpauses scene
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            reticle.enabled = true;
        }
    }

    public void updateGoalEnemy(int amount) // Temporary win condition based on defeat # of enemies
    { // Use this for testing and possibly for the first demo
        enemyCount += amount; 
        enemyCountText.text = enemyCount.ToString("F0");

        if (enemyCount <= 0)
        {
            menuProcess(menuWin);
        }
    }

    public void updateGoalSplat(int amount) // Temporary win condition based on splatter
    { // WIP
        splatCount += amount;
        splatCountText.text = splatCount.ToString("F0");

        if (splatCount == 10) // Temp goal is "splatting" 10 walls
        {
            menuProcess(menuWin);
        }
    }

    public void updateLose() // Lose menu processing
    {
        menuProcess(menuLose);
    }

    public void updateLevelUp()
    {
        menuProcess(menuLevelUp);
    }
    public void DisplayStats()
    {
        // Current Damage of Gun
        float currentGunDamage = playerScript.shootDamage;
        statDamageCurrGun.text = currentGunDamage.ToString("F0");

        // Damage Multiplier
        float damageMult = playerScript.GetDamageMult();
        
        // Spaghetti code heaven (fixes a rounding error???)
        float total = currentGunDamage * damageMult;
        statTotal.text = total.ToString();
        float corredtDisplay = total / currentGunDamage;
        statDamageMult.text = corredtDisplay.ToString();

        // Speed
        statSpeed.text = instance.playerMovementScript.GetSpeed().ToString("F0");

        // Number of Jumps
        statNumJumps.text = instance.playerMovementScript.GetJumps().ToString("F0");
    }
    public void UpdateAmmo()
    {
        WeaponStats currWeap = playerScript.GetSelectedWeapon();
        weaponName.text = currWeap.WeaponName.ToString();

        if (currWeap.isAmmoInfinite)
        {
            ammoCurrent.text = "∞";
            ammoMax.text = "∞";
            return;
        }

        ammoCurrent.text = currWeap.ammo.ToString();
        ammoMax.text = currWeap.ammoMax.ToString();
        
    }
}
