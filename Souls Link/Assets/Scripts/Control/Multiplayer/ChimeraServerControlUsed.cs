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

                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                Debug.Log("agrega el delegado");
                GetComponent<ChimeraMultiplayerController>().enabled = true;
                GetComponent<ChimeraMultiplayerController>().addDelegate();
                break;
        }
    }
}
