using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashBar : MonoBehaviour
{
    public GameObject Canvas;
    [Header("Charge bar")]
    public GameObject _UIGameObject;
    private GameObject temp;
    public Camera _camera;
    [HideInInspector]
    public GameObject chargeBar;
    public float offsetY = 0;
    public SmashAttack _dashScript;

    Vector2 posItem = Vector2.zero;

    private void Start()
    {
        chargeBar = Instantiate(_UIGameObject, Canvas.transform);
        _dashScript.setSmashBar(chargeBar);
        chargeBar.GetComponent<Image>().fillAmount = 0;
    }

    /*
    void Update()
    {
        if (_camera == null)
        {
            posItem = Camera.main.WorldToScreenPoint(transform.position);
        }
        else
        {
            posItem = _camera.WorldToScreenPoint(transform.position);
        }
        chargeBar.transform.position = posItem;
    }
    */
}
