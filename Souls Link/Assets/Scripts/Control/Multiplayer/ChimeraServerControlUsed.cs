using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraServerControlUsed : CharacterServerControlUsed
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
                //Destroy(GetComponent<ChimeraMultiplayerController>());
                //GetComponent<PhotonChimeraMultiplayerController>().enabled = true;
                //GetComponent<PhotonChimeraMultiplayerController>().addDelegate();
                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                //Destroy(GetComponent<PhotonChimeraMultiplayerController>());
                GetComponent<ChimeraMultiplayerController>().enabled = true;
                GetComponent<ChimeraMultiplayerController>().addDelegate();
                break;
        }
    }
}
