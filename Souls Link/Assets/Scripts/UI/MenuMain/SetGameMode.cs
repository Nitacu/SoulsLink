using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameMode : MonoBehaviour
{
   public void setOnlineMode()
    {
        GameManager.GetInstace()._gameMode = GameManager.GameMode.ONLINE;
    }

    public void setOfflineMode()
    {
        GameManager.GetInstace()._gameMode = GameManager.GameMode.OFFLINE;
    }
}
