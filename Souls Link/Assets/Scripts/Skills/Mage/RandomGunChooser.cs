using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        
    }

    public void startChoosing()
    {
        randomPicker = Instantiate(_UIGameObject, Canvas.transform);
        randomPicker.GetComponent<RandomNumberPicker>().setRandomizer(gameObject);
        randomPicker.GetComponent<RandomNumberPicker>().randomize();
    }

    public void recieveGunData(float gunNumber)
    {
        Debug.Log("Picked gun: " + gunNumber);
    }

    void Update()
    {
        if (randomPicker != null)
        {
            Vector3 namePos = Camera.main.WorldToScreenPoint(transform.position);
            randomPicker.transform.position = namePos;
        }
    }
}
