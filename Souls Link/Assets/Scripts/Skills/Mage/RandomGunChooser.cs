using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomGunChooser : MonoBehaviour
{
    public GameObject Canvas;
    [Header("RandomPickerPrefab")]
    public GameObject _UIGameObject;
    private GameObject temp;
    public Camera _camera;
    [HideInInspector]
    public GameObject randomPicker;
    public float offsetY = 0;
    private Vector2 posItem = Vector2.zero;
    public PhotonView _photonView;


    private void Start()
    {
        
    }

    public void startChoosing()
    {
        if (GetComponentInParent<PhotonCharacterMultiplayerController>().isMine())
        {
            randomPicker = Instantiate(_UIGameObject, Canvas.transform);
            randomPicker.GetComponent<RandomNumberPicker>().setRandomizer(gameObject);
            randomPicker.GetComponent<RandomNumberPicker>().NumberWeapon = Random.Range(1, 9);
            randomPicker.GetComponent<RandomNumberPicker>().randomize();
        }
    }

    [PunRPC]
    public void recieveGunData(float gunNumber)
    {
        Debug.Log("Picked gun: " + gunNumber);

        if (gunNumber % 2 == 0)
        {
            GetComponentInParent<RandomGun>().spawnEnergyBalls();
        }
        else
        {
            GetComponentInParent<RandomGun>().spawnTurret();
        }
        //


        //GetComponentInParent<RandomGun>().canPickAgain();
    }

    void Update()
    {
        /*
        if (randomPicker != null)
        {
            if (_camera == null)
            {
                posItem = Camera.main.WorldToScreenPoint(transform.position);
            }
            else
            {
                posItem = _camera.WorldToScreenPoint(transform.position);
            }
            randomPicker.transform.position = posItem;
        }
        */
    }
}
