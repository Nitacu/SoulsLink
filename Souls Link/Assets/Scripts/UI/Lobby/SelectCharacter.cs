using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private GameObject _selectingCharacterPrefab;
    [SerializeField] private List<PlayerSelectCharPanel> _imagePlayers = new List<PlayerSelectCharPanel>();
    [SerializeField] private float _offsetX = 215f;
    [SerializeField] private List<PlayerSelectCharPanel> _slots = new List<PlayerSelectCharPanel>();
    private int _indexSlotsFilled = 0;

    private List<GameObject> currentLocalPlayer = new List<GameObject>();
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

    private string myID;
    public string MyID { get => myID; set => myID = value; }

    private void OnEnable()
    {
        //Si está multiplayer buscar cuantos hay players hay actualmente en este room, con base a esa informacion actualizar el indexSlotsFilled


        //Si no es multiplayer simplemente actualizar la información lista de slots
        StartCoroutine(SelectPanelByList());


        //Multiplayer old method - cambiar
        //StartCoroutine(FindAndSelectMyPanel());
    }

    IEnumerator SelectMyIndex()
    {
        yield return new WaitForEndOfFrame();

        int playersNumber = 0;

        ControlLobbyUI control = FindObjectOfType<ControlLobbyUI>();
        if (control != null)
        {
            playersNumber = control.playersNumber();
        }
    }

    IEnumerator SelectPanelByList()
    {
        yield return new WaitForEndOfFrame();

        PlayerInput.Instantiate(_selectingCharacterPrefab);
        //GameObject playerInstace = Instantiate(_selectingCharacterPrefab);
    }

    /*


IEnumerator FindAndSelectMyPanel()
{
    yield return new WaitForEndOfFrame();

    Debug.Log("Find players ID");
    foreach (PlayerSelectCharPanel imagePlayer in _imagePlayers)
    {
        if (imagePlayer.isActiveAndEnabled)
        {
            imagePlayer.DeactivateMySelection();

            string panelID = imagePlayer.PlayerID;
            if (panelID != null)
            {
                if (panelID.Equals(MyID))
                {
                    //set seleccionar personaje
                    Debug.Log("Finded my self: " + MyID);
                    _charactersPanel = imagePlayer.getCharacterSelection();
                    _leftArrow = imagePlayer.getLeftArrow();
                    _rightArrow = imagePlayer.getRightArrow();
                    break;
                }
            }
        }
    }

    resetCharacterPanelPosition();
    selectCharacter(_characterIndexSelected);
    setArrows();
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

public void OnStartGame()
{
    starGame();
}
*/

    public void OnJoinGame()
    {
        Debug.Log("create new player");

        StartCoroutine(SelectPanelByList());
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("On Player Joined: " + input.gameObject);
        setUpNewSelectingPlayer(input.gameObject);
    }

    public void setUpNewSelectingPlayer(GameObject player)
    {
        SelectingCharacter selectCharComp = player.GetComponent<SelectingCharacter>();
        currentLocalPlayer.Add(player);

        int count = 0;
        foreach (var slot in _slots)
        {
            if (!slot.IsFilled)//encontrar el primero que tenga espacio
            {
                //Añadirme y llenar
                slot.NewPlayerID = count;
                slot.IsFilled = true;

                selectCharComp.setCharSelection(slot.getCharacterSelection(),
                    slot.getLeftArrow(), slot.getRightArrow());

                /*
                _charactersPanel = slot.getCharacterSelection();
                _leftArrow = slot.getLeftArrow();
                _rightArrow = slot.getRightArrow();
                */
                break;
            }
            count++;
        }

        if (currentLocalPlayer.Count <= 1)//PONER CONTROL
        {

        }
    }
}

