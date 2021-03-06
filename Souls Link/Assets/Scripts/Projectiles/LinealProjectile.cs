﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinealProjectile : Projectile
{
    [SerializeField] private float _speed;
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    private Vector2 _velocity;
    public Vector2 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    private GameObject _playerReference;
    public GameObject Player
    {
        get { return _playerReference; }
        set { _playerReference = value; }
    }
    public float Damage { get; set; }

    [SerializeField] private bool _destrutable = true;

    private void Update()
    {
        Vector2 currentPos = gameObject.transform.position;
        Vector2 newPos = currentPos + _velocity * _speed * Time.deltaTime;

        Debug.DrawLine(currentPos, newPos, Color.red);
        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPos, newPos);


        foreach (var hit in hits)
        {

            GameObject other = hit.collider.gameObject;
            if (other != Player)
            {
                if (other.CompareTag("Wall"))
                {
                    Destroy(gameObject);
                    break;
                }
                if (other.CompareTag("Player") && _projetileOwner != ProjectileOwner.PLAYER)
                {
                    //Aply Damage
                    other.GetComponent<PlayerHPControl>().recieveDamage(Damage, gameObject);
                    Destroy(gameObject);
                    break;
                }
            }
        }

        transform.position = newPos;

    }


    public void setRotation(Vector2 direction)
    {
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //gameObject.transform.Rotate(0, 0, rot - 90);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    public override void reflectMySelf()
    {
        Debug.Log("Reflect");
        this._reflected = true;
        //Vector2 newDirection = Vector2.Reflect(_velocity, normalDirection);
        Vector2 newDirection = (_velocity * -1);
        newDirection.Normalize();
        _velocity = newDirection;
        setRotation(newDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _projetileOwner != ProjectileOwner.ENEMY)
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);

            if (_destrutable)
            {
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player") && _projetileOwner != ProjectileOwner.PLAYER)
        {
            //Aply Damage
            Debug.Log("Aply Damage");
            if (collision.GetComponent<PlayerHPControl>())
            {
                collision.GetComponent<PlayerHPControl>().recieveDamage(Damage, gameObject);
            }
            else if (collision.GetComponent<SelfDestroy>())
            {
                collision.GetComponent<SelfDestroy>().loseHealth(20);
            }

            Destroy(gameObject);
        }
    }
}
