using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : NetworkBehaviour
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // for(int i = 0; i < players.Length; i++) {
        //     players[i].GetComponent<movement>().DisableListening();
        // }
        NetworkManager.Singleton.Shutdown();
        Destroy(GameObject.Find("NetworkManager"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
