using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomController : MonoBehaviour
{
    public GameObject _room1;
    public GameObject _room2;
    public GameObject _room3;
    public GameObject _room4;
    public GameObject _room5;

    public GameObject _doorRoom2;
    public GameObject _doorRoom3;
    public GameObject _doorRoom4;
    public GameObject _doorRoom5;

    private List<GameObject> targetsRoom1 = new List<GameObject>();
    private List<GameObject> targetsRoom2 = new List<GameObject>();
    private List<GameObject> targetsRoom3 = new List<GameObject>();
    private List<GameObject> targetsRoom4 = new List<GameObject>();
    private List<GameObject> targetsRoom5 = new List<GameObject>();

    public bool door2Opened = false;
    public bool door3Opened = false;
    public bool door4Opened = false;
    public bool door5Opened = false;

    public GameObject oppositeRoom;
    public GameObject winScreen;

    public bool ChimeraOne = false;

    // Start is called before the first frame update
    void Start()
    {
        fillRooms();
    }

    public void checkDoorsToOpen(GameObject _dummyReference, DummyController.roomOfDummy _room)
    {
        switch (_room)
        {
            case DummyController.roomOfDummy.ROOM1:
                targetsRoom1.Remove(_dummyReference);
                break;
            case DummyController.roomOfDummy.ROOM2:
                targetsRoom2.Remove(_dummyReference);
                break;
            case DummyController.roomOfDummy.ROOM3:
                targetsRoom3.Remove(_dummyReference);
                break;
            case DummyController.roomOfDummy.ROOM4:
                targetsRoom4.Remove(_dummyReference);
                break;
            case DummyController.roomOfDummy.ROOM5:
                targetsRoom5.Remove(_dummyReference);
                break;
        }

        if(targetsRoom1.Count < 1 && !door2Opened)
        {
            _room2.SetActive(true);
            _doorRoom2.SetActive(false);
            door2Opened = true;
        }
        if (targetsRoom2.Count < 1 && !door3Opened)
        {
            _room3.SetActive(true);
            _doorRoom3.SetActive(false);
            door3Opened = true;
        }
        if (targetsRoom3.Count < 1 && !door4Opened)
        {
            _room4.SetActive(true);
            _doorRoom4.SetActive(false);
            door4Opened = true;
        }
        if (targetsRoom4.Count < 1 && !door5Opened)
        {
            _room5.SetActive(true);
            _doorRoom5.SetActive(false);
            door5Opened = true;
        }
        if (targetsRoom5.Count < 1 )
        {
            //end game
            oppositeRoom.SetActive(false);
            winScreen.SetActive(true);
            if (ChimeraOne)
            {
                winScreen.GetComponentInChildren<TextMeshProUGUI>().text = "QUIMERA 1 WINS!";
                winScreen.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
            }
            else
            {
                winScreen.GetComponentInChildren<TextMeshProUGUI>().text = "QUIMERA 2 WINS!";
                winScreen.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
        }
    }

    

    private void fillRooms()
    {
        foreach(Transform child in _room1.transform)
        {
            if(child.tag != "EditorOnly")
                targetsRoom1.Add(child.gameObject);
        }
        foreach (Transform child in _room2.transform)
        {
            if (child.tag != "EditorOnly")
                targetsRoom2.Add(child.gameObject);
        }
        foreach (Transform child in _room3.transform)
        {
            if (child.tag != "EditorOnly")
                targetsRoom3.Add(child.gameObject);
        }
        foreach (Transform child in _room4.transform)
        {
            if (child.tag != "EditorOnly")
                targetsRoom4.Add(child.gameObject);
        }
        foreach (Transform child in _room5.transform)
        {
            if (child.tag != "EditorOnly")
                targetsRoom5.Add(child.gameObject);
        }

        _room2.SetActive(false);
        _room3.SetActive(false);
        _room4.SetActive(false);
        _room5.SetActive(false);
    }
}
