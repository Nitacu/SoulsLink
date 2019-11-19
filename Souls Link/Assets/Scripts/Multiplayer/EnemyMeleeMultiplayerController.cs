using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class EnemyMeleeMultiplayerController : MonoBehaviour
{
    [SerializeField] private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;

    private void Start()
    {
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
    }

    public bool isMine()
    {
        return _networkID.IsMine;
    }
}
