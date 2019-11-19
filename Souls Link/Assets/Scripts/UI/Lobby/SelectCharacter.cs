using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using UnityEngine.InputSystem;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private List<PlayerSelectCharPanel> _imagePlayers = new List<PlayerSelectCharPanel>();
    [SerializeField] private float _offsetX = 215f;
    private int _characterIndexSelected = 0;
    private GameObject _charactersPanel;

    private void OnEnable()
    {
        StartCoroutine(FindAndSelectMyPanel());
    }

    IEnumerator FindAndSelectMyPanel()
    {
        yield return new WaitForEndOfFrame();

        string myID = NetworkClient.Lobby.PlayerId;

        Debug.Log("Find players ID");
        foreach (PlayerSelectCharPanel imagePlayer in _imagePlayers)
        {
            if (imagePlayer.isActiveAndEnabled)
            {
                imagePlayer.DeactivateMySelection();

                string panelID = imagePlayer.PlayerID;
                if (panelID.Equals(myID))
                {
                    //set seleccionar personaje
                    Debug.Log("Finded my self: " + myID);
                    _charactersPanel = imagePlayer.getCharacterSelection();
                    break;
                }
            }
        }
    }

    public void OnSelectRight()
    {
        //moveSelectionToRight
        if (_characterIndexSelected < (_imagePlayers.Count - 1))//puede moverse a al derecha
        {
            _characterIndexSelected++;

            //MoverPanel HaciaIzquierda
            movePanel(true);
        }
    }

    public void OnSelectLeft()
    {
        //moveSelectionLeft
        if (_characterIndexSelected > 0)
        {
            _characterIndexSelected--;
            Debug.Log("Elegir char izquierda Index: " + _characterIndexSelected);
            //MoverPanelDerecha
            movePanel(false);
        }
    }

    public void movePanel(bool selectRight)
    {
        float currentXPos = _charactersPanel.GetComponent<RectTransform>().localPosition.x;
        float newPos = (selectRight) ? currentXPos - _offsetX : currentXPos + _offsetX;
        _charactersPanel.GetComponent<RectTransform>().localPosition = new Vector3(newPos, 0);
    }

    public void OnStartGame()
    {
        selectCharacter();
        FindObjectOfType<LobbyKevin>().StartGame();
    }

    public void selectCharacter()
    {
        GameManager.Characters _characterEnum;

        switch (_characterIndexSelected)
        {
            case 0:
                _characterEnum = GameManager.Characters.MAGE;
                break;
            case 1:
                _characterEnum = GameManager.Characters.TANK;
                break;
            case 2:
                _characterEnum = GameManager.Characters.DRUID;
                break;
            case 3:
                _characterEnum = GameManager.Characters.ASSASIN;
                break;
            default:
                _characterEnum = GameManager.Characters.MAGE;
                break;
        }

        GameManager.GetInstace()._myCharacter = _characterEnum;
    }

}
