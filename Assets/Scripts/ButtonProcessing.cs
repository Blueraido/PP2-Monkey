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

    public void expSpeed()
    {
        GameManager.instance.playerScript.speed += 1;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
