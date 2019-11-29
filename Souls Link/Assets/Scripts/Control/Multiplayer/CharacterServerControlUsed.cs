using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using Photon.Realtime;
using Photon.Pun;

public class CharacterServerControlUsed : MonoBehaviour
{
    [Header("Socketweaver")]
    [SerializeField] private NetworkID _networkID;
    [SerializeField] private RemoteEventAgent _eventAgent;
    [SerializeField] private RealtimeAgent _realtimeAgent;
    [SerializeField] private SyncPropertyAgent _propertyAgent;
    [Header("Photon")]
    [SerializeField] private PhotonTransformView _transformView;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private PhotonAnimatorView _animatorView;

    private void OnEnable()
    {
        switch (GameManager.GetInstace()._multiplayerServer)
        {
            case GameManager.MultiplayerServer.PHOTON:
                if (_eventAgent)
                    Destroy(_eventAgent);

                if (_realtimeAgent)
                    Destroy(_realtimeAgent);

                if (_propertyAgent)
                    Destroy(_propertyAgent);

                if (_networkID)
                    Destroy(_networkID);

                GetComponent<PhotonCharacterMultiplayerController>().enabled = true;
                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                if (_transformView)
                    Destroy(_transformView);

                if (_animatorView)
                    Destroy(_animatorView);

                if (_photonView)
                    Destroy(_photonView);

                GetComponent<CharacterMultiplayerController>().enabled = true;
                break;
        }
    }
}
