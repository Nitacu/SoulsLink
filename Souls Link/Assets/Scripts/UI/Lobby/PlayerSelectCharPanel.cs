using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
    [SerializeField] private int _indexSlot;
    [SerializeField] public GameObject ownCharacterSelection;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;
    [SerializeField] TMP_Text _nickName;

    private void Awake()
    {

        ownCharacterSelection.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    [PunRPC]
    public void loadNickName(string nickName)
    {
        _nickName.text = nickName;
    }

    [PunRPC]
    public void DeactivateMySelection()
    {
        ownCharacterSelection.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        IsFilled = false;
        GetComponentInParent<SelectCharacter>().unlockASlot(_indexSlot);
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
