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
    private NetworkVariable<int> PlayerOneScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> PlayerTwoScore = new NetworkVariable<int>(0);
    public List<ulong> playerIDList = new List<ulong>();
    public WorldItemStorage worldItems;
    int winAmount = 1;


    // Start is called before the first frame update
    void Awake()
    {
        UpdateScreenScoreBoard();
        worldItems = GameObject.Find("World Items").GetComponent<WorldItemStorage>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++) {
            playerIDList.Add(players[i].GetComponent<movement>().OwnerClientId);
        }
    }
    void FixedUpdate() {
        UpdateScreenScoreBoard();
        if(PlayerOneScore.Value == winAmount) {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length; i++) {
                    if(playerIDList[0] == players[i].GetComponent<movement>().OwnerClientId) {
                        players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("DefeatScreen");
                    }
                    else {
                        players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("VictoryScreen");
                    }
                }
            }
            else if(PlayerTwoScore.Value == winAmount) {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length; i++) {
                    if(playerIDList[1] == players[i].GetComponent<movement>().OwnerClientId) {
                        players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("DefeatScreen");

                    }
                    else {
                        players[i].GetComponent<movement>().playerCam.GetComponent<PlayerController>().openUI("VictoryScreen");
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
        worldItems.player.GetComponent<movement>().playerCam.GetComponent<PlayerController>().closeUI();
    }
    [Rpc(SendTo.Server)]
    public void RespawnResquestRPC(ulong playerID) {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //finds the player with the right tag and then respawns them
        for(int i = 0; i < players.Length; i++) {
            if(playerID == players[i].GetComponent<movement>().OwnerClientId) {
                players[i].transform.position = players[i].GetComponent<movement>().spawnPoint.position;
            }
        }

    }
}
