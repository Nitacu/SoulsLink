using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectCharPanel : MonoBehaviour
{
    private string _playerId;
    public string PlayerID
    {
        get { return _playerId; }
        set { _playerId = value; }
    }

    [SerializeField] GameObject ownCharacterSelection;

    public void DeactivateMySelection()
    {
        ownCharacterSelection.SetActive(false);
    }

    public GameObject getCharacterSelection()
    {
        ownCharacterSelection.SetActive(true);
        return ownCharacterSelection;
    }

}
