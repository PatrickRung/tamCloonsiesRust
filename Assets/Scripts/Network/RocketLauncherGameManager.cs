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
}
