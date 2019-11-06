using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZordMove : MonoBehaviour
{
    private List<string> _listAxisX = new List<string>();
    private List<string> _listAxisY = new List<string>();
    private float _moveHorizontal;
    private float _moveVertical;
    private Vector2 _movement;
    private const float SPEED_BASE = 5;

    #region GET Y SET
    public List<string> ListAxisX { get => _listAxisX; set => _listAxisX = value; }
    public List<string> ListAxisY { get => _listAxisY; set => _listAxisY = value; }
    #endregion GET Y SET

    private void FixedUpdate()
    {
        move();
        adjustingMotionAnimations();
    }

    private void move()
    {
        //movimiento del player
        checkAxisX();
        checkAxisY();
        _movement = new Vector2(_moveHorizontal, _moveVertical);
        GetComponent<Rigidbody2D>().velocity = _movement * SPEED_BASE;
    }

    private bool checkAxisX()
    {
        _moveHorizontal = Input.GetAxis(_listAxisX[0]);

        foreach (string axis in _listAxisX)
        {
            //o ambos son mayores o ambos son menore
            if (!((_moveHorizontal>0 && Input.GetAxis(axis)>0) || (_moveHorizontal < 0 && Input.GetAxis(axis) < 0)))
            {
                _moveHorizontal = 0;
                return false;
            }
        }
        return true;
    }

    private bool checkAxisY()
    {
        _moveVertical = Input.GetAxis(_listAxisY[0]);

        foreach (string axis in _listAxisY)
        {
            //o ambos son mayores o ambos son menore
            if (!((_moveVertical <0 && Input.GetAxis(axis) < 0) || (_moveVertical > 0 && Input.GetAxis(axis) > 0)))
            {
                _moveVertical = 0;
                return false;
            }
        }
        return true;
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
