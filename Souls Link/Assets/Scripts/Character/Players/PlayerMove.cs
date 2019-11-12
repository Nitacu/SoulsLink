using SWNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveHorizontal;
    private float _moveVertical;
    private Vector2 _movement;
    public Vector2 Movement
    {
        get { return _movement; }
    }

    private const float SPEED_BASE = 3;
    private bool isDashing = false;

    private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;

    private const string MOVE = "move";

    private Vector2 lastDirection = Vector2.zero;

    #region para probar todo con teclado

    private string _axisX;
    private string _axisY;
    [SerializeField] private player _typePlayer;


    #region GET Y SET
    public string AxisX { get => _axisX; set => _axisX = value; }
    public string AxisY { get => _axisY; set => _axisY = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool IsDashing1 { get => isDashing; set => isDashing = value; }
    #endregion

    private enum player
    {
        PLAYER_1,
        PLAYER_2
    }

    public void selectInputs()
    {
        switch (_typePlayer)
        {
            case player.PLAYER_1:
                AxisX = "Horizontal";
                AxisY = "Vertical";
                break;

            case player.PLAYER_2:
                AxisX = "Horizontal 2";
                AxisY = "Vertical 2";
                break;
        }
    }

    #endregion

    private void Start()
    {
        selectInputs();
        _networkID = GetComponent<NetworkID>();
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
    }

    private void FixedUpdate()
    {
        if (_networkID.IsMine)
        {
            _moveHorizontal = Input.GetAxis(AxisX);
            _moveVertical = Input.GetAxis(AxisY);
            _movement = new Vector2(_moveHorizontal, _moveVertical);

            SWNetworkMessage message = new SWNetworkMessage();
            message.Push(_movement);

            //lo envia a las otras maqunas
            _remoteEventAgent.Invoke(MOVE, message);
            //activa el mio
            move();
        }
    }

    public void move()
    {
        //movimiento del player
        if (!isDashing)
        {
            GetComponent<Rigidbody2D>().velocity = _movement * SPEED_BASE;
            if (_movement != Vector2.zero)
            {
                lastDirection = _movement.normalized;
                GetComponent<AimCursor>().LastVector = lastDirection;
            }
        }
        else
        {
            GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.LastVector);
            
        }
        adjustingMotionAnimations();
    }

    public void move(SWNetworkMessage message)
    {
        this._movement = message.PopVector3();
        this._moveHorizontal = _movement.x;
        this._moveVertical = _movement.y;

        //movimiento del player
        GetComponent<Rigidbody2D>().velocity = _movement * SPEED_BASE;
        adjustingMotionAnimations();
    }

    public void adjustingMotionAnimations()
    {
        //coloca la animacion
        GetComponent<Animator>().SetFloat("Velocity", _movement.magnitude);

        //rota el sprite
        if (_moveHorizontal < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (_moveHorizontal > 0)
            GetComponent<SpriteRenderer>().flipX = false;
    }
}