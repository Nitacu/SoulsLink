using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingCharacter : MonoBehaviour
{
    [SerializeField] private float _offsetX = 110f;

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
        resetCharacterPanelPosition();
        selectCharacter(_characterIndexSelected);
        setArrows();
    }

    public void setCharSelection(GameObject charactersPanel, GameObject leftArrow, GameObject rightArrow)
    {
        _charactersPanel = charactersPanel;
        _leftArrow = leftArrow;
        _rightArrow = rightArrow;
    }

    public void OnSelectRight()
    {
        //moveSelectionToRight
        if (_characterIndexSelected < (System.Enum.GetValues(typeof(GameManager.Characters)).Length - 1))//puede moverse a al derecha
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
            (_characterIndexSelected < (System.Enum.GetValues(typeof(GameManager.Characters)).Length - 1)) ? true : false
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
}
