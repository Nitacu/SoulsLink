using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public float health = 0;
    public bool canWalk = false;
    public float movementSpeed = 0;
    private bool facingLeft = false;
    private Rigidbody2D _rb;
    private bool firstTimePressing = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            canWalk = !canWalk;
            if (firstTimePressing)
            {
                firstTimePressing = false;
                changeDirection();
            }
                
        }

        if (canWalk)
        {
            moveEnemy();
        }
        else{
            _rb.velocity = Vector3.zero;
        }
    }

    private void changeDirection()
    {
        facingLeft = !facingLeft;
        Invoke("changeDirection", 5);
    }

    private void moveEnemy()
    {
        Vector2 direction = Vector2.zero;

        if (facingLeft)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
        _rb.velocity = direction * movementSpeed;
    }

    public void recieveDamage(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            Destroy(gameObject);
        }
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("backToNormal", 0.5f);
    }

    private void backToNormal()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void stopWalking()
    {
        canWalk = false;
    }
}
