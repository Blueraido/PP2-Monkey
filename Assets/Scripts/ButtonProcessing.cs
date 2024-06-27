using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonProcessing : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.menuProcess(); // Resumes the game on button
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Loads the scene
        GameManager.instance.menuProcess();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void fullHeal()
    {
        GameManager.instance.playerScript.AddHealth(GameManager.instance.playerScript.GetHealthMax());
    }

    // Exp buttons for leveling below
    // Replace actual funcs with getters / setters for player eventually
    // Not everything is guaranteed to be used, consider it a secret for dataminers to find lol
    public void expButtonDamage()
    {
        GameManager.instance.playerScript.AddDamageMult(0.5f);
        fullHeal();
        GameManager.instance.menuProcess();
        Debug.Log("Damage Increased!");
    }
    public void expButtonHealth()
    {
        GameManager.instance.playerScript.AddHealthMax(5); // adds health here, full heals below
        fullHeal();
        GameManager.instance.menuProcess();
        Debug.Log("Health Increased!");
    }
    public void expButtonSpeed()
    {
        GameManager.instance.playerMovementScript.AddSpeed(1);
        fullHeal();
        GameManager.instance.menuProcess();
        Debug.Log("Speed Increased!");
    }
    public void expButtonStamina()
    {
        GameManager.instance.playerMovementScript.AddStaminaMax(5);
        GameManager.instance.playerMovementScript.AddStamina((int)GameManager.instance.playerMovementScript.GetStaminaMax());
        fullHeal();
        GameManager.instance.menuProcess();
        Debug.Log("Stamina Increased!");
    }
    public void expButtonJumps()
    {
        GameManager.instance.playerMovementScript.AddJumps(1);
        fullHeal();
        GameManager.instance.menuProcess();
        Debug.Log("Number of Jumps Increased!");
    }
}
