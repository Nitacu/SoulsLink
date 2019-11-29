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
    private Vector2 _inputMovement;

    [SerializeField] private float _speed = 100;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _renderer;
    const string VELOCITY_PARAMETER = "Velocity";

    [HideInInspector]
    public bool isDashing = false;


    private FusionTrigger _fusionTriggerRef;
    

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
        if (_fusionTriggerRef != null)
        {
            if (_fusionTriggerRef.IsOnFusion)
            {
                moveOnFusion();
                return;
            }
        }

        move();
    }



    private void setAnimation()
    {
        //RotarSprite
        if (_inputMovement.x > 0)
        {
            Renderer.flipX = false;
        }
        else if (_inputMovement.x < 0)
        {
            Renderer.flipX = true;
        }

    }

    private void moveOnFusion()
    {
        _fusionTriggerRef.CurrentChimeraParent.sendMovement(_inputMovement, _fusionTriggerRef.OnFusionID);        
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
                GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.AimDirection); //Dash asesino (el que dashea bastante como un rayo)
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
            //para mover el player en esta maquina

            StartCoroutine(multiplayerDelay(context.Get<Vector2>()));
            //InputMovement = context.Get<Vector2>();
            
            
            //llama decirle a las otras maquinas que tienen que mover este PJ
        }
    }

     IEnumerator multiplayerDelay(Vector2 input)
    {
        yield return new WaitForSeconds(DELAY);

        InputMovement = input;

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


