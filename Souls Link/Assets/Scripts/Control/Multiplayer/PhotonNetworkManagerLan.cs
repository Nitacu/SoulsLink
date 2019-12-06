using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonNetworkManagerLan : PhotonNetworkManager
{
    public string _serverAddress;
    public int _port;

    public override void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Conectando al sevidor...");
            PhotonNetwork.ConnectToMaster(_serverAddress, _port, "1");
        }
    }

  
}
