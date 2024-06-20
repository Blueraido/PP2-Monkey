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

    // Exp buttons for leveling below
    // Replace actual funcs with getters / setters for player eventually
    // Not everything is guaranteed to be used, consider it a secret for dataminers to find lol
    public void expButtonDamage()
    {
        Debug.Log("Damage Increased!");
        //GameManager.instance.playerScript.stamina += 5;
        GameManager.instance.menuProcess();
    }
    public void expButtonHealth()
    {
        Debug.Log("Health Increased!");
        //GameManager.instance.playerScript.HP += 5;
        GameManager.instance.menuProcess();
    }
    public void expButtonSpeed()
    {
        Debug.Log("Speed Increased!");
        //GameManager.instance.playerScript.speed += 1;
        GameManager.instance.menuProcess();
    }
    public void expButtonStamina()
    {
        Debug.Log("Stamina Increased!");
        //GameManager.instance.playerScript.stamina += 5;
        GameManager.instance.menuProcess();
    }
    public void expButtonJumps()
    {
        Debug.Log("Number of Jumps Increased!");
        //GameManager.instance.playerScript.jumps += 1;
        GameManager.instance.menuProcess();
    }
}
