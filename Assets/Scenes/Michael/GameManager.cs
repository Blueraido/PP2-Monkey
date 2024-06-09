using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Obtain player references
    GameManager instance;
    public GameObject player;

    // Replace this with whatever player controller winds up being:
    // public PlayerController playerScript;

    // Hud element processing
    public Image playerHPBar;
    public Image playerStaminaBar;

    // Pause menu processing
    bool isPaused;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    // Objective text ui processing
    [SerializeField] TMP_Text enemyCount;
    [SerializeField] TMP_Text splatCount;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");

        // Replace this with player script reference:
        // playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null) // Pauses the game when cancel is pressed and current active menu is null
            {
                menuProcess(menuPause); // Makes current menu the pause menu
            }

            else if (menuActive == menuPause) // Allows only pause menu to be escaped out of
            {
                menuProcess(null); // Makes current menu null
            }
        }
    }

    public void menuProcess(GameObject menu)
    {
        togglePause();
        menuActive = menu;
        menuActive.SetActive(isPaused);
    }

    public void togglePause()
    {
        isPaused = !isPaused; // Inverts pause state here

        if (isPaused) // Pauses scene
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            menuActive.SetActive(isPaused);
        }

        else if (!isPaused) // Unpauses scene
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            menuActive.SetActive(isPaused); // activates menu based on isPaused bool
            menuActive = null;
        }
    }
}
