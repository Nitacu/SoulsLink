using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private float _distance = 2;

    private PlayerInputActions _inputControl;

    private Vector2 _aimDirection = Vector2.right;
    public Vector2 AimVector
    {
        get { return _aimDirection; }
    }

    private void Awake()
    {
        _inputControl = new PlayerInputActions();
    }

    private void Start()
    {
        if (!GetComponent<CharacterMultiplayerController>().isMine())
        {
            Destroy(_crossHair);
            Destroy(GetComponent<PlayerAiming>());
        }
    }

    private void Update()
    {
        SetCrossHair();
    }

    private void SetCrossHair()
    {
        if (_aimDirection.magnitude > 0)
        {
            _crossHair.transform.localPosition = _aimDirection * _distance;
            _crossHair.SetActive(true);
        }
        else
        {
            _crossHair.SetActive(false);
        }

    }

    public void OnMove(InputValue context)
    {
        //Vector2 value = context.ReadValue<Vector2>();

        if (context.Get<Vector2>() != Vector2.zero)
        {
            //_aimDirection = value;
            _aimDirection = context.Get<Vector2>();

        }

    }

    //enables
    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Enable();
    }
}
