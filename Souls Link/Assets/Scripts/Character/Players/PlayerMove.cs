using SWNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveHorizontal;
    private float _moveVertical;
    private Vector2 _movement;
    private const float SPEED_BASE = 3;
    private NetworkID _networkID;

    #region para probar todo con teclado

    private string _axisX;
    private string _axisY;
    [SerializeField] private player _typePlayer;


    #region GET Y SET
    public string AxisX { get => _axisX; set => _axisX = value; }
    public string AxisY { get => _axisY; set => _axisY = value; }
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
    }

    private void FixedUpdate()
    {
        if (_networkID.IsMine)
        {
            move();
            adjustingMotionAnimations();
        }
    }

    public void move()
    {
        //movimiento del player
        _moveHorizontal = Input.GetAxis(AxisX);
        _moveVertical = Input.GetAxis(AxisY);
        _movement = new Vector2(_moveHorizontal, _moveVertical);
        GetComponent<Rigidbody2D>().velocity = _movement * SPEED_BASE;
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