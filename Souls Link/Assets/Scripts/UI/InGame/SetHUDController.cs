using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHUDController : MonoBehaviour
{
    [SerializeField] private GameObject _leftHUD;
    [SerializeField] private GameObject _rightHUD;

    public void setLeftHUD()
    {
        _leftHUD.SetActive(true);
        _rightHUD.SetActive(false);
    }

    public void setRightHUD()
    {
        _leftHUD.SetActive(false);
        _rightHUD.SetActive(true);
    }
}
