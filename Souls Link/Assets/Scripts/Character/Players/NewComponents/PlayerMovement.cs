using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterMultiplayerController _characterMultiplayerController;
    private PlayerInputActions _inputControl;
    private Rigidbody2D _rb;
    private Vector2 _inputMovement;

    [SerializeField] private float _speed = 100;
    [SerializeField] private Animator _anim;
    const string VELOCITY_PARAMETER = "Velocity";

    private void Awake()
    {
        _inputControl = new PlayerInputActions();

        _characterMultiplayerController = GetComponent<CharacterMultiplayerController>();
        _rb = GetComponent<Rigidbody2D>();
        //_anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        _rb.velocity = InputMovement * _speed * Time.deltaTime;
        _anim.SetFloat(VELOCITY_PARAMETER, _rb.velocity.sqrMagnitude);

    }

    public void OnMove(InputValue context)
    {
        // _inputMovement = context.ReadValue<Vector2>();
        if (_characterMultiplayerController.isMine())
        {
            //para mover el player en esta maquina
            InputMovement = context.Get<Vector2>();
            //llama decirle a las otras maquinas que tienen que mover este PJ
            _characterMultiplayerController.pushVectorMovement(context.Get<Vector2>());
        }            
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

    /////////////// GET Y SET //////////////////////////////
    public Vector2 InputMovement { get => _inputMovement; set => _inputMovement = value; }
}


