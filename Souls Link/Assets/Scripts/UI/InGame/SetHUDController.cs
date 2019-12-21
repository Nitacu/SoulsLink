using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHUDController : MonoBehaviour
{
    [SerializeField] private GameObject _leftHUD;
    [SerializeField] private GameObject _rightHUD;

    private HUDOrientation _orientation;

    public enum HUDOrientation
    {
        LEFT,
        RIGHT
    }

    public void setLeftHUD()
    {
        Orientation = HUDOrientation.LEFT;
        _leftHUD.SetActive(true);
        _rightHUD.SetActive(false);
    }

    public void setRightHUD()
    {
        Orientation = HUDOrientation.RIGHT;
        _leftHUD.SetActive(false);
        _rightHUD.SetActive(true);
    }

    public void setHUDWithParameter(HUDOrientation orientation)
    {
        switch (orientation)
        {
            case HUDOrientation.LEFT:
                setLeftHUD();
                break;

            case HUDOrientation.RIGHT:
                setRightHUD();
                break;
        }
    }

    public void ActivateHUD(bool activate)
    {
        switch (Orientation)
        {
            case HUDOrientation.LEFT:
                _leftHUD.SetActive(activate);
                _rightHUD.SetActive(false);
                break;

            case HUDOrientation.RIGHT:
                _leftHUD.SetActive(false);
                _rightHUD.SetActive(activate);
                break;
        }
    }

    public HUDOrientation Orientation { get => _orientation; set => _orientation = value; }
}
