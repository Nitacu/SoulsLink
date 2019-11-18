using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClampText : MonoBehaviour
{
    public GameObject Canvas;
    [Header ("Charge bar")]
    public GameObject _UIGameObject;
    private GameObject temp;
    public Camera _camera;
    [HideInInspector]
    public GameObject chargeBar;
    public float offsetY = 0;

    private void Start()
    {
        chargeBar = Instantiate(_UIGameObject, Canvas.transform);
        chargeBar.GetComponent<Image>().fillAmount = 0;
    }

    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(transform.position);
        chargeBar.transform.position = namePos;
    }
}

