using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Realtime;
using Photon.Pun;

public class EnemyServerControlUsed : CharacterServerControlUsed
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void activeMultiplayerController()
    {
        switch (GameManager.GetInstace()._multiplayerServer)
        {
            case GameManager.MultiplayerServer.PHOTON:
                Destroy(GetComponent<EnemyMeleeMultiplayerController>());
                GetComponent<PhotonEnemyMultiplayerController>().enabled = true;
                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                Destroy(GetComponent<PhotonEnemyMultiplayerController>());
                GetComponent<EnemyMeleeMultiplayerController>().enabled = true;
                break;
        }
    }
}
