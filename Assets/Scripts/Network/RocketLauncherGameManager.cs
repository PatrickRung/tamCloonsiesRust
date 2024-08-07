using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

public class RocketLauncherGameManager : NetworkBehaviour
{
    public NetworkVariable<int> PlayerOneScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> PlayerTwoScore = new NetworkVariable<int>(0);
    public List<ulong> playerIDList = new List<ulong>();
    public WorldItemStorage worldItems;
    public int winAmount = 5;
    public bool gameOver;


    // Start is called before the first frame update
    void Awake()
    {
        gameOver = false;
        UpdateScreenScoreBoard();
        worldItems = GameObject.Find("World Items").GetComponent<WorldItemStorage>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++) {
            playerIDList.Add(players[i].GetComponent<movement>().OwnerClientId);
        }
    }
    void FixedUpdate() {
        UpdateScreenScoreBoard();
        if(PlayerOneScore.Value >= winAmount) {
                gameOver = true;
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length; i++) {
                    if(players[i].GetComponent<movement>().playerCam != null) {
                        if(playerIDList[0] == players[i].GetComponent<movement>().OwnerClientId) {
                            players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("DefeatScreen");
                        }
                        else {
                            players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("VictoryScreen");
                        }
                    }
                }
            }
            else if(PlayerTwoScore.Value >= winAmount) {
                gameOver = true;
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length; i++) {
                    if(players[i].GetComponent<movement>().playerCam != null) {
                        if(playerIDList[1] == players[i].GetComponent<movement>().OwnerClientId) {
                            players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("DefeatScreen");

                        }
                        else {
                            players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("VictoryScreen");
                        }
                    }

                }
            }
    }
    [Rpc(SendTo.Server)]
    public void UpdateScoreBoardRPC(ulong player) {
        if(player == playerIDList[0]) {
            PlayerOneScore.Value++;
        }
        else if(player == playerIDList[1]) {
            PlayerTwoScore.Value++;
        }

    }    
    void UpdateScreenScoreBoard() {
        gameObject.GetComponent<TextMeshProUGUI>().text = PlayerOneScore.Value + " Player One -------- Player Two " + PlayerTwoScore.Value;
    }
    public void Respawn() {
        RespawnResquestRPC(worldItems.player.GetComponent<movement>().OwnerClientId);
        worldItems = GameObject.Find("World Items").GetComponent<WorldItemStorage>();
        worldItems.player.GetComponent<movement>().playerCam.GetComponent<PlayerController>().closeUI();
    }
    [Rpc(SendTo.Everyone)]
    public void RespawnResquestRPC(ulong playerID) {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        worldItems = GameObject.Find("World Items").GetComponent<WorldItemStorage>();
        //finds the player with the right tag and then respawns them
        for(int i = 0; i < players.Length; i++) {
            Debug.Log("how many times we do");
            if(worldItems.player.GetComponent<movement>().OwnerClientId == players[i].GetComponent<movement>().OwnerClientId) {
                players[i].transform.position = players[i].GetComponent<movement>().spawnPoint.position;
                players[i].GetComponent<movement>().Position.Value = players[i].GetComponent<movement>().spawnPoint.position;
                players[i].GetComponent<movement>().respawn();
                players[i].GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            }
        }

    }
}
