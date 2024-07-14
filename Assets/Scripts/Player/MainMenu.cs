using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void FireRange()
    {
        SceneManager.LoadScene("FiringRange");
    }
    public void NewGame()
    {
        SceneManager.LoadScene("LEVEL1");
    }
    public void RocketLauncherGame()
    {
        SceneManager.LoadScene("RocketLauncherGame");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
