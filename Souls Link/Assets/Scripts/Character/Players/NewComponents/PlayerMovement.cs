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


    private FusionTrigger _fusionTriggerRef;


    private ChimeraController _currentChimeraParent;
    public ChimeraController CurrentChimeraParent
    {
        get { return _currentChimeraParent; }
        set { _currentChimeraParent = value; }
    }

    private void Awake()
    {
        _inputControl = new PlayerInputActions();
        _fusionTriggerRef = GetComponent<FusionTrigger>();
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
        if (_fusionTriggerRef != null)
        {
            if (_fusionTriggerRef.IsOnFusion)
            {
                moveOnFusion();
            }
            else
            {
                move();
            }
        }
        else
        {
            move();
        }
    }



    private void setAnimation()
    {
        _anim.SetFloat(VELOCITY_PARAMETER, _rb.velocity.sqrMagnitude);

        //RotarSprite
        if (_inputMovement.x > 0)
        {
            Renderer.flipX = false;
        }
        else if (_inputMovement.x < 0)
        {
            Renderer.flipX = true;
        }

        if (_characterMultiplayerController.isMine())
            _characterMultiplayerController.changeFlip(Renderer.flipX);
    }

    private void moveOnFusion()
    {
        _currentChimeraParent.sendMovement(_inputMovement, _fusionTriggerRef.OnFusionID);
    }


    private void move()
    {
        if (!isDashing)
        {
            _rb.velocity = InputMovement * _speed * Time.deltaTime;
        }
        else
        {
            if (GetComponent<Dash>() != null)
            {
                GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.AimDirection);
            }
            else
            {
                GetComponent<CometDash>().playerDash(GetComponent<CometDash>().Aiming.AimDirection);
            }
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
    public SpriteRenderer Renderer { get => _renderer; set => _renderer = value; }
}


