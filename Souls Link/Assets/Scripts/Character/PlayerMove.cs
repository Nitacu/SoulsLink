using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveHorizontal;
    private float _moveVertical;
    private Vector2 _movement;
    private const float SPEED_BASE_X = 3;


    private void FixedUpdate()
    {
        move();
        adjustingMotionAnimations();
    }


    public void move()
    {
        //movimiento del player
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
        _movement = new Vector2(_moveHorizontal, _moveVertical);
        GetComponent<Rigidbody2D>().velocity = _movement * SPEED_BASE_X;
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
