using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region CharacterInfo
    //enum characters
    public enum Characters
    {
        TANK, MAGE, DRUID, ASSASIN
    }
    #endregion

    //Character Selected in Lobby
    public Characters _myCharacter;


    public enum MultiplayerTypeConnection
    {
        HOST,
        INVITED
    }
    public MultiplayerTypeConnection _multiplayerConnection;

    public enum GameMode
    {
        OFFLINE,
        ONLINE
    }
    public GameMode _gameMode;

    public List<Characters> _charactersList = new List<Characters>();

    static GameManager _instace;
    public static GameManager GetInstace()
    {
        if (_instace == null)
        {
            _instace = new GameManager();
        }

        return _instace;
    }


}
