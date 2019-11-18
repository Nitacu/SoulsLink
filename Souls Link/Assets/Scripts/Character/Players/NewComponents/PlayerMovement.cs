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
    [SerializeField] private SpriteRenderer _renderer;
    const string VELOCITY_PARAMETER = "Velocity";

    [HideInInspector]
    public bool isDashing = false;

    private void Awake()
    {
        _inputControl = new PlayerInputActions();

        _characterMultiplayerController = GetComponent<CharacterMultiplayerController>();
        _rb = GetComponent<Rigidbody2D>();
        //_anim = GetComponent<Animator>();
    }
    private void Update()
    {
        setAnimation();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void setAnimation()
    {
        _anim.SetFloat(VELOCITY_PARAMETER, _rb.velocity.sqrMagnitude);

        //RotarSprite
        if (_inputMovement.x > 0)
        {
            _renderer.flipX = false;
        }
        else if (_inputMovement.x < 0)
        {
            _renderer.flipX = true;
        }
    }

    private void move()
    {
        if (!isDashing)
        {
            _rb.velocity = InputMovement * _speed * Time.deltaTime;
        }
        else
        {
            GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.AimDirection);
        }
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


