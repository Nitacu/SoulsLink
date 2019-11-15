using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions _inputControl;
    private Rigidbody2D _rb;
    private Vector2 _inputMovement;

    [SerializeField] private float _speed = 100;
    private Animator _anim;
    const string VELOCITY_PARAMETER = "Velocity";

    private void Awake()
    {
        _inputControl = new PlayerInputActions();

        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        _rb.velocity = _inputMovement * _speed * Time.deltaTime;
        _anim.SetFloat(VELOCITY_PARAMETER, _rb.velocity.sqrMagnitude);
    }

    public void OnMove(InputValue context)
    {
        // _inputMovement = context.ReadValue<Vector2>();
        _inputMovement = context.Get<Vector2>();
    }

    //Enable and Disable
    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }
}


