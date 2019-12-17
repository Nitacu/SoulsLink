using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class SelectCharacter : MonoBehaviour
{
    public PhotonView _photonView;

    [SerializeField] private GameObject _selectingCharacterPrefab;
    [SerializeField] private List<PlayerSelectCharPanel> _imagePlayers = new List<PlayerSelectCharPanel>();
    [SerializeField] private float _offsetX = 215f;
    [SerializeField] private List<PlayerSelectCharPanel> _slots = new List<PlayerSelectCharPanel>();
    public List<bool> _filledSlots = new List<bool>();
    private int _indexSlotsFilled = 0;

    [SerializeField]private List<GameObject> currentLocalPlayer = new List<GameObject>();
    public List<GameObject> CurrentLocalPlayer
    {
        get { return currentLocalPlayer; }
    }

    private int _characterIndexSelected = 0;
    private GameObject _charactersPanel;
    private GameObject _leftArrow;
    private GameObject _rightArrow;

    public delegate void StarGame();
    public StarGame starGame;

    private int _lastPlayerIndex;
    private string myID;
    public string MyID { get => myID; set => myID = value; }

    private void OnEnable()
    {
        //Si no es multiplayer simplemente actualizar la información lista de slots
        ControlLobbyUI control = FindObjectOfType<ControlLobbyUI>();

        foreach (var slot in _slots)
        {
            slot.gameObject.SetActive(true);
        }
        SetToOldVersion(false);

        if (PhotonNetwork.IsMasterClient)
        {
            setFilledSlots();

            StartCoroutine(SelectPanelByList());

        }
        else
        {
            //colocar algo mientras espera respuesta

            //activar panel de espera

            _photonView.RPC("sendFilledSlotList", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
            Debug.Log("Pedir lista a master");
        }
    }

    private void OnDisable()
    {
        currentLocalPlayer.Clear();
    }


    #region RPC's
    [PunRPC]
    public void sendFilledSlotList(Photon.Realtime.Player player)
    {
        //convertir list a string de 1 y 0
        string binaryChain = "";
        
        List<bool> auxList = new List<bool>();
        for (int i = 0; i < _filledSlots.Count; i++)
        {
            auxList.Add(_filledSlots[i]);
        }

        // quita el ultimo player que entro a la sala para que se envie la lista sin el player que esta 
        //pidiendo la lista 
        auxList[_lastPlayerIndex] = false; 

        foreach (var slot in auxList)
        {
            binaryChain += (slot) ? "1" : "0";
        }
        _photonView.RPC("sendFilledSlotListOnline", player, binaryChain);
    }

    [PunRPC]
    public void sendFilledSlotListOnline(string binaryChainList)
    {
        List<bool> newBooleanList = new List<bool>();
        //convertir string a lista
        foreach (char _char in binaryChainList)
        {
            if (_char == '1')
            {
                newBooleanList.Add(true);
            }
            else if (_char == '0')
            {
                newBooleanList.Add(false);
            }
        }

        _filledSlots = newBooleanList;
        StartCoroutine(SelectPanelByList());

        //desactivar panel de espera
    }

    #endregion

    #region GENERAL_CONTROL
    private void setFilledSlots()
    {
        _filledSlots.Clear();

        foreach (var slot in _slots)
        {
            _filledSlots.Add(slot.IsFilled);
        }
    }

    private void SetToOldVersion(bool setOldVersion)
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        PlayerInputManager playerInputManager = GetComponent<PlayerInputManager>();

        if (playerInput != null)
        {
            playerInput.enabled = setOldVersion;
        }

        if (playerInputManager != null)
        {
            playerInputManager.enabled = !setOldVersion;
        }

    }


    public void unlockASlot(int index)
    {
        _filledSlots[index] = false;
    }
    #endregion

    #region PANELS_SELECTION
    IEnumerator SelectPanelByList()
    {
        yield return new WaitForEndOfFrame();

        PlayerInput.Instantiate(_selectingCharacterPrefab);
    }

    public void setUpNewSelectingPlayer(GameObject player)
    {
        if (currentLocalPlayer.Count >= 1)//segundo jugador
        {
            _photonView.RPC("addPlayerUpdate", RpcTarget.Others);
        }

        SelectingCharacter selectCharComp = player.GetComponent<SelectingCharacter>();
        currentLocalPlayer.Add(player);

        bool slotFinded = false;

        for (int i = 0; i < _filledSlots.Count; i++)
        {
            if (!_filledSlots[i] && !slotFinded)//encontrar el primero que tenga espacio
            {
                Debug.Log("Find slot empty at: " + i);
                //Añadirme y llenar
                _slots[i].NewPlayerID = i;
                _slots[i].IsFilled = true;
                _filledSlots[i] = true;

                slotFinded = true;

                Debug.Log("SelectCharacter. Set new player selecting" + i);
                selectCharComp.setCharSelection(_slots[i].getCharacterSelection(),
                    _slots[i].getLeftArrow(), _slots[i].getRightArrow(), currentLocalPlayer.Count - 1);

                break;
            }
            else
            {
                Debug.Log("Slot filled at " + i);
                _slots[i].ownCharacterSelection.SetActive(true);
            }
        }



    }

    [PunRPC]
    public void addPlayerUpdate()
    {
        Debug.Log("añadir player externo");

        for (int i = 0; i < _filledSlots.Count; i++)
        {
            if (!_filledSlots[i])//encontrar el primero que tenga espacio
            {
                _lastPlayerIndex = i;
                _filledSlots[i] = true;
                _slots[i].IsFilled = true;
                _slots[i].ownCharacterSelection.SetActive(true);
                break;
            }
        }
    }
    #endregion

    #region PLAYER_MANAGER

    public void OnStartGame()
    {
        starGame();
    }

    public void OnJoinGame()
    {
        Debug.Log("create new player");

        StartCoroutine(SelectPanelByList());
    }

    public void onLeaveRoom()
    {
        currentLocalPlayer.Clear();

        foreach (PlayerSelectCharPanel panel in _imagePlayers)
        {
            panel.IsFilled = false;
        }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        if (currentLocalPlayer.Count == 0)//primer jugador setear a teclado
        {
            InputDevice[] devices = new InputDevice[1];
            var keyboards = InputSystem.FindControls("<Keyboard>");
            var gamepads = InputSystem.FindControls("<Gamepad>");

            if (gamepads.Count > 0)//hay gamepads
            {
                devices[0] = gamepads[0].device;

            }
            else//no hay gamepads usar teclado
            {
                devices[0] = keyboards[0].device;
            }

            input.SwitchCurrentControlScheme(devices);
        }

        setUpNewSelectingPlayer(input.gameObject);
    }
    #endregion

    #region HANDLE_SLOT
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
            //MoverPanelDerecha
            movePanel(false);
        }
    }

    public void movePanel(bool selectRight)
    {
        selectCharacter(_characterIndexSelected);
        setArrows();

        float currentXPos = _charactersPanel.GetComponent<RectTransform>().localPosition.x;
        float newPos = (selectRight) ? currentXPos - _offsetX : currentXPos + _offsetX;
        _charactersPanel.GetComponent<RectTransform>().localPosition = new Vector3(newPos, 0);
    }

    public void setArrows()
    {
        //set Right Arrow
        _rightArrow.SetActive(
            (_characterIndexSelected < (_imagePlayers.Count - 1)) ? true : false
            );

        //set Left Arrow
        _leftArrow.SetActive(
            (_characterIndexSelected > 0) ? true : false
            );
    }

    public void selectCharacter(int currentIndex)
    {
        GameManager.Characters _characterEnum;

        switch (currentIndex)
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

    public void resetCharacterPanelPosition()
    {
        _characterIndexSelected = 0;
        //_charactersPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0);
    }
    #endregion
}

