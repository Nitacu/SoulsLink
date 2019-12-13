using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    const float DELAY = 0.05f;

    [SerializeField] private GameObject _crossHair;
    [SerializeField] private float _distance = 2;
    [SerializeField] private float _offsetY = 0.358f;

    [HideInInspector]
    public Vector2 playerPosition = Vector2.zero;

    private PlayerInputActions _inputControl;

    private Vector2 _aimDirection = Vector2.right;

    #region delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    #endregion

    private void Awake()
    {
        _inputControl = new PlayerInputActions();
    }

    public Vector2 getPosition()
    {
        playerPosition = gameObject.transform.position;
        playerPosition.y = playerPosition.y + _offsetY;
        return playerPosition;
    }

    public float getOffsetY()
    {
        return _offsetY;
    }

    private void Start()
    {
        StartCoroutine(destroyCrossHair());
    }

    IEnumerator destroyCrossHair()
    {
        yield return new WaitForEndOfFrame();

        if (!_isMine())
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
        if (gameObject.GetComponent<FusionTrigger>())
        {
            if (gameObject.GetComponent<FusionTrigger>().IsOnFusion)
            {
                _crossHair.SetActive(false);
                return;
            }
        }

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
        if (_isMine())
        {
            if (context.Get<Vector2>() != Vector2.zero)
            {

                Vector2 newAimAux = context.Get<Vector2>();
                newAimAux.Normalize();

                StartCoroutine(setDirectionDelay(newAimAux));

                /*
                //_aimDirection = value;

                AimDirection = context.Get<Vector2>();
                AimDirection.Normalize();

                //le envia a los otras maquinas a donde apunta este personaje
                
                _characterMultiplayerController.pushVectorAiming(AimDirection);
                */
            }
        }
    }

    IEnumerator setDirectionDelay(Vector2 _aim)
    {
        yield return new WaitForSeconds(DELAY);

        AimDirection = _aim;
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
