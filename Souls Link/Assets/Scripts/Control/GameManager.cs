using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region CharacterInfo
    //enum characters
    public enum Characters
    {
        TANK, MAGE, DRUID, ASSASIN, NONE
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

    public Characters[] _charactersList = new Characters[2] {Characters.NONE, Characters.NONE};

    public CharacterSetUp[] _charactersSetupList = new CharacterSetUp[2];


    static GameManager _instace;
    public static GameManager GetInstace()
    {
        if (_instace == null)
        {
            _instace = new GameManager();

            //Instanciar chararcetrlistsetup
            for (int i = 0; i < _instace._charactersSetupList.Length; i++)
            {
                _instace._charactersSetupList[i] = new CharacterSetUp();
            }
        }

        return _instace;
    }
}

public class CharacterSetUp
{
    public string device = "";
    public GameManager.Characters characterType = GameManager.Characters.NONE;        
}


