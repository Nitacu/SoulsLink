using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    const float DELAY = 0.05f;

    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;

    private PlayerInputActions _inputControl;
    private Rigidbody2D _rb;
    public Rigidbody2D RigidBodyPlayer
    {
        get { return _rb; }
        set { _rb = value; }
    }
    private Vector2 _inputMovement = Vector2.zero;    

    [SerializeField] private float _speed = 100;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _renderer;
    const string VELOCITY_PARAMETER = "Velocity";

    [HideInInspector]
    public bool isDashing = false;
    public bool _flip = false;

    private FusionTrigger _fusionTriggerRef;
    public FusionTrigger FusionTriggerRef
    {
        get { return _fusionTriggerRef; }
    }


    private void Awake()
    {
        _inputControl = new PlayerInputActions();
        _fusionTriggerRef = GetComponent<FusionTrigger>();
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

        changeOrientation();
    }


    public void changeOrientation()
    {
        if (!_isMine())
        {
            _anim.GetComponent<SpriteRenderer>().flipX = _flip;
        }
    }

    private void setAnimation()
    {
        //RotarSprite
        if (_inputMovement.x > 0)
        {
            Renderer.flipX = false;
            _flip = false;
        }
        else if (_inputMovement.x < 0)
        {
            Renderer.flipX = true;
            _flip = true;
        }  
    }

    private void move()
    {        
        if (!isDashing)
        {
            //movimiento normal
            if (!moveAsChimera())
            {
                _rb.velocity = InputMovement * _speed * Time.deltaTime;
                _anim.SetFloat(VELOCITY_PARAMETER, _rb.velocity.magnitude);
            }
        }
        else
        {
            if (GetComponent<Dash>() != null)
            {
                if (!GetComponent<Dash>().isSimpleDash)
                {
                    GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.AimDirection); //Dash asesino (el que dashea bastante como un rayo)
                }
                else
                {
                    GetComponent<CometDash>().playerDash(GetComponent<CometDash>().Aiming.AimDirection);
                }
            }
            else 
            {
                GetComponent<CometDash>().playerDash(GetComponent<CometDash>().Aiming.AimDirection); //Dash basico
            }
        }
    }

    public void OnMove(InputValue context)
    {
        // _inputMovement = context.ReadValue<Vector2>();
        if (_isMine())
        {
            if (context.Get<Vector2>() != InputMovement)
            {
                StartCoroutine(multiplayerDelay(context.Get<Vector2>()));
            }
        }
    }

    IEnumerator multiplayerDelay(Vector2 input)
    {
        yield return new WaitForSeconds(DELAY);

        InputMovement = input;

        //Si tengo que enviar a chimera el movimiento
        if (moveAsChimera() && _isMine())
        {
            //_fusionTriggerRef.CurrentChimeraParent.sendMovement(InputMovement, _fusionTriggerRef.OnFusionID);
            //se lo envia solo al host
            _fusionTriggerRef.CurrentChimeraParent.sendMovement(InputMovement, _fusionTriggerRef.OnFusionID);
        }

    }

    public bool moveAsChimera()
    {
        bool sendMovementToChimera = false;
        if (_fusionTriggerRef != null)
        {
            if (_fusionTriggerRef.IsOnFusion)
            {
                sendMovementToChimera = true;
            }
        }
        return sendMovementToChimera;
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


