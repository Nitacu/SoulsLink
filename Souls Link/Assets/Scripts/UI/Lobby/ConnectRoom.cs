using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectRoom : MonoBehaviour
{
    public string _id;
    private PhotonNetworkManager _photonNetwork;

    void Start()
    {
        _photonNetwork = FindObjectOfType<PhotonNetworkManager>();
    }

    public void conectRoom()
    {
        _photonNetwork.loginRoom(_id);
    }

}
