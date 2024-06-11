using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Obtain player references
    public static GameManager instance;
    public GameObject player;

    // Player script reference:
    public PlayerController playerScript;

    // Hud element processing
    [SerializeField] public Image playerHPBar;
    [SerializeField] public Image playerStaminaBar;

    // Pause menu processing
    bool isPaused;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    // Objective text ui processing
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text splatCountText;
    int enemyCount;
    int splatCount;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null) // Pauses the game when cancel is pressed and current active menu is null
                menuProcess(menuPause); // Makes current menu the pause menu

            else if (menuActive == menuPause) // Allows only pause menu to be escaped out of
                menuProcess(); // Unpause is done by passing in nothing

            else { } // Do nothing if other assumed menu is on screen (only pause should be esc out of)
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void menuProcess(GameObject menu = null)
    {
        if (menu != null)
        {
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
        }

        else if (!isPaused) // Unpauses scene
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

    public void updateGoalSplat(int amount) // Temporary win condition based on walls splattered
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
}
