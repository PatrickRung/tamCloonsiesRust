using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//lot of code from this tutorial https://www.youtube.com/watch?v=msPNJ2cxWfw&t=230s&ab_channel=CodeMonkey
//refer to it if anything bugs out

public class NetworkManagingUI : NetworkBehaviour
{

    private Lobby hostLobby;
    GameObject lobbyCodeInput;

    private float heartBeatTimer;
    //async allows the rest of the game to continue working while await function 
    //waits for return

    private void Update() {
        HandleHeartBeatTimer();
    }
    private void HandleHeartBeatTimer() {
        if(hostLobby!=null) {
            heartBeatTimer -= Time.deltaTime;
            if(heartBeatTimer < 0f) {


                //heartBeatTimers allow the server to stay alive because if the lobby is innactive for more than 30 seconds it will auto close
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    //authentication
    private async void Start() {
        //assigning 
        lobbyCodeInput = GameObject.Find("LobbyCodeInput");

        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();



    }

    public async void CreateRelay() {
        try {

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            String JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(JoinCode);
            lobbyCodeInput.GetComponent<InputField>().text = JoinCode;
            lobbyCodeInput.GetComponent<InputField>().readOnly = true;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, 
                (ushort) allocation.RelayServer.Port, 
                allocation.AllocationIdBytes, 
                allocation.Key,
                allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();
        } catch(RelayServiceException e) {
            Debug.Log(e);
        }

    }

    public async void CreateLobby() {
        try {
            // string lobbyName = "MyLobby";
            // int maxPlayers = 4;
            // Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            // hostLobby = lobby;

            // Debug.Log("created lobby " + lobby.Name + " " + lobby.MaxPlayers);

            CreateRelay();
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void ListLobbies() {
        try {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results) {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } 
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinLobby() {

        try {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results) {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        } 
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }

    }

    public async void JoinRelay(string joinCode) {

        try {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation =  await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
              joinAllocation.RelayServer.IpV4, 
            (ushort) joinAllocation.RelayServer.Port, 
            joinAllocation.AllocationIdBytes, 
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
        } 
        catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }

    public void startGame() {
        NetworkManager.Singleton.SceneManager.LoadScene("RocketLauncherGame", LoadSceneMode.Single);
    }


}
