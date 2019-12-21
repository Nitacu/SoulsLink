using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallController : MonoBehaviour
{

    private bool canMove = false;
    Vector2 _direction = Vector2.zero;
    float _speed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            GetComponentInParent<Rigidbody2D>().velocity = _direction * _speed;
        }
    }

    public void startMoving()
    {
        canMove = true;
    }

    public void setWall(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
