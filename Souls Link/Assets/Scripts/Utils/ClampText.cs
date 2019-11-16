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
    public float offsetY = 0;

    private void Start()
    {
        temp = Instantiate(_UIGameObject, Canvas.transform);
        temp.GetComponent<Image>().fillAmount = 0;
    }

    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
        temp.transform.position = namePos;
    }
}

