using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonSpawnPlayer : MonoBehaviourPun
{
    public PhotonView _photonView;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private List<string> _characters = new List<string>();

    private void Start()
    {

        PhotonNetwork.Instantiate(_characters[0], new Vector3(0f, 0, 0f), Quaternion.identity, 0);

    }
}
