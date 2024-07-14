using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RocketLauncherGameManager : MonoBehaviour
{

    private NetworkManager m_NetworkManager;
    // Start is called before the first frame update
    void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
        {
            StartButtons();
        }
        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host")) {
            m_NetworkManager.StartHost();
            GameObject.Find("playerCam").GetComponent<PlayerController>().Awake();
        } 
        if (GUILayout.Button("Client")) {
            m_NetworkManager.StartClient();
            GameObject.Find("playerCam").GetComponent<PlayerController>().Awake();
        }
        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
    }
}
