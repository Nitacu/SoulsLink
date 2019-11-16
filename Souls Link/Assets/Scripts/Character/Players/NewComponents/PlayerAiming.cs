using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private float _distance = 2;

    private CharacterMultiplayerController _characterMultiplayerController;
    private PlayerInputActions _inputControl;

    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _characterMultiplayerController = GetComponent<CharacterMultiplayerController>();
        _inputControl = new PlayerInputActions();
    }

    private void Start()
    {
        if (!_characterMultiplayerController.isMine())
        {
            _crossHair.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        SetCrossHair();
    }

    private void SetCrossHair()
    {
        if (AimDirection.magnitude > 0)
        {
            _crossHair.transform.localPosition = AimDirection * _distance;
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
        if (_characterMultiplayerController.isMine())
        {
            if (context.Get<Vector2>() != Vector2.zero)
            {
                //_aimDirection = value;
                AimDirection = context.Get<Vector2>();
                //le envia a los otras maquinas a donde apunta este personaje
                _characterMultiplayerController.pushVectorAiming(AimDirection);
            }
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

    ///////////////////////////////GET Y SET ////////////////////////////////
    public Vector2 AimDirection { get => _aimDirection; set => _aimDirection = value; }
}
