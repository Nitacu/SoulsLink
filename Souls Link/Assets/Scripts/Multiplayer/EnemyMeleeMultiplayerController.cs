using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class EnemyMeleeMultiplayerController : MonoBehaviour
{
    [SerializeField] private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;
    private SyncPropertyAgent _syncPropertyAgent;
    private const string POSITION = "Position";

    private void Start()
    {
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _syncPropertyAgent = GetComponent<SyncPropertyAgent>();
    }

    public bool isMine()
    {
        if (!_networkID.IsMine)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        return _networkID.IsMine;
    }

    private void Update()
    {
        if (isMine())
        {
            //transform.position = _syncPropertyAgent.GetPropertyWithName(POSITION).GetVector3Value();
            _syncPropertyAgent.Modify(POSITION, transform.position);
        }
            
    }

    #region movimiento y posicion 
    public void OnPositionReady()
    {
        Debug.Log("OnHPPropertyReady");

        if (isMine())
        {
            _syncPropertyAgent.Modify(POSITION, transform.position);
        }

    }

    public void OnPositionChanged()
    {
        // Update the hpSlider when player hp changes
        transform.position = _syncPropertyAgent.GetPropertyWithName(POSITION).GetVector3Value();

    }
    #endregion
}
