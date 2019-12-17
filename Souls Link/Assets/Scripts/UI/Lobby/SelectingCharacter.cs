using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SelectingCharacter : MonoBehaviour
{
    [SerializeField] private float _offsetX = 110f;

    private int _characterIndexSelected = 0;
    [SerializeField] private GameObject _charactersPanel;
    private GameObject _leftArrow;
    private GameObject _rightArrow;

    public delegate void StarGame();
    public StarGame starGame;
    private int myID;

    GameManager.Characters charSelected;

    private void OnEnable()
    {
        resetCharacterPanelPosition();
        selectCharacter(_characterIndexSelected);
        setArrows();
    }

    // para cargar mi nick name
    private void Start()
    {
        if (FindObjectOfType<SelectCharacter>().CurrentLocalPlayer.Count == 1)
            _charactersPanel.GetComponentInParent<PlayerSelectCharPanel>()._photonView.RPC("loadNickName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
    }

    //borra el texto
    private void OnDestroy()
    {
        _charactersPanel.GetComponentInParent<PlayerSelectCharPanel>()._photonView.RPC("loadNickName", RpcTarget.AllBuffered, "");
    }

    public void setCharSelection(GameObject charactersPanel, GameObject leftArrow, GameObject rightArrow, int id)
    {
        Debug.Log("Character Selection setted");
        CharactersPanel = charactersPanel;
        _leftArrow = leftArrow;
        _rightArrow = rightArrow;
        MyID = id;
    }

    public void OnSelectRight()
    {
        //moveSelectionToRight
        if (_characterIndexSelected < (System.Enum.GetValues(typeof(GameManager.Characters)).Length - 2))//puede moverse a al derecha
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
        StartCoroutine(setArrows());

        float currentXPos = CharactersPanel.GetComponent<RectTransform>().localPosition.x;
        float newPos = (selectRight) ? currentXPos - _offsetX : currentXPos + _offsetX;
        CharactersPanel.GetComponent<RectTransform>().localPosition = new Vector3(newPos, 0);

        //enviar mi selección a otras máquinas
        CharactersPanel.GetComponentInParent<PlayerSelectCharPanel>()._photonView.RPC("setCharacterPanelPosition", Photon.Pun.RpcTarget.OthersBuffered, new Vector3(newPos, 0));
    }

    IEnumerator setArrows()
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("Set Arrows");

        //set Right Arrow
        _rightArrow.SetActive(
            (_characterIndexSelected < (System.Enum.GetValues(typeof(GameManager.Characters)).Length - 2)) ? true : false
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
                _characterEnum = GameManager.Characters.NONE;
                break;
        }

        GameManager.GetInstace()._charactersList[MyID] = _characterEnum;

        //guardar tipo y bind
        Debug.Log("Selecting Character . Select character . MyId; " + MyID);
        GameManager.GetInstace()._charactersSetupList[MyID].characterType = _characterEnum;

        string device = gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0].name;
        GameManager.GetInstace()._charactersSetupList[MyID].device = device;

        string scheme = gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>().currentControlScheme;
        GameManager.GetInstace()._charactersSetupList[MyID].scheme = scheme;


    }

    public void resetCharacterPanelPosition()
    {
        _characterIndexSelected = 0;
        //_charactersPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0);
    }

    public void OnStartGame()
    {
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene("PracticeRange");
        }
    }

    /////////////////////////////GET Y SET ////////////////////////
    public int MyID { get => myID; set => myID = value; }
    public GameObject CharactersPanel { get => _charactersPanel; set => _charactersPanel = value; }
}
