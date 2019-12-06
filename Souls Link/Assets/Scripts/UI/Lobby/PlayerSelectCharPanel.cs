using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSelectCharPanel : MonoBehaviour
{
    public PhotonView _photonView;

    public string _playerId;
    public string PlayerID
    {
        get { return _playerId; }
        set { _playerId = value; }
    }

    private int _newPlayerId;
    public int NewPlayerID
    {
        get { return _newPlayerId; }
        set { _newPlayerId = value; }
    }

    public bool _isFilled = false;
    public bool IsFilled
    {
        get { return _isFilled; }
        set { _isFilled = value; }
    }

    [SerializeField] public GameObject ownCharacterSelection;
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

    [PunRPC]
    public void setCharacterPanelPosition(Vector3 newPos)
    {
        ownCharacterSelection.GetComponent<RectTransform>().localPosition = newPos;
    }
}
