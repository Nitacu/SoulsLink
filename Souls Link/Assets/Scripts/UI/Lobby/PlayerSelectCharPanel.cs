using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectCharPanel : MonoBehaviour
{
    public string _playerId;
    public string PlayerID
    {
        get { return _playerId; }
        set { _playerId = value; }
    }

    [SerializeField] GameObject ownCharacterSelection;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;

    private void Awake()
    {
        ownCharacterSelection.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    public void DeactivateMySelection()
    {
        ownCharacterSelection.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    public GameObject getCharacterSelection()
    {
        ownCharacterSelection.SetActive(true);
        return ownCharacterSelection;
    }

    public GameObject getLeftArrow()
    {
        leftArrow.SetActive(true);
        return leftArrow;
    }

    public GameObject getRightArrow()
    {
        rightArrow.SetActive(true);
        return rightArrow;
    }

}
