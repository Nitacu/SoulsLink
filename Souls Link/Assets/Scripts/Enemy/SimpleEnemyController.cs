﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;


public class SimpleEnemyController : MonoBehaviour
{
    public float health = 0;
    public bool canWalk = true;
    private bool facingLeft = false;
    private Rigidbody2D _rb;
    private bool firstTimePressing = true;
    private bool isGettingDamaged = false;
    private Animator _anim;

    public Animator Anim { get => _anim; set => _anim = value; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Anim.SetBool("Can_Walk", canWalk);
    }

    public void changeOrientation(float value)
    {
        if (value > 0)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = true;
    }

    public void attack(GameObject player)
    {
        player.GetComponent<PlayerHPControl>().recieveDamage(20, gameObject);
        Anim.Play(Animator.StringToHash("Attack"));
    }

    public void recieveDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Anim.Play(Animator.StringToHash("Death"));
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            StartCoroutine(die());
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
            Invoke("backToNormal", 0.5f);
        }
    }

    public void stopDamage()
    {
        isGettingDamaged = false;
    }

    public void timeDamage(float time)
    {
        isGettingDamaged = true;
        StartCoroutine(stopTickDamage(time));
    }

    IEnumerator die()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<ControlSpawnEnemys>().spawnNewEnemy();
        Destroy(gameObject);
    }

    public void recieveTickDamage(float damage, float tickTime)
    {

        if (isGettingDamaged)
        {
            health -= damage;
            if (health < 0)
            {
                Anim.Play(Animator.StringToHash("Death"));
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StartCoroutine(die());
            }
            else
            {
                StartCoroutine(recieveTick(damage, tickTime));
                GetComponent<SpriteRenderer>().color = Color.red;
                Invoke("backToNormal", tickTime / 2);
            }
        }
        Debug.Log(health);
    }

    IEnumerator recieveTick(float _damage, float tickTime)
    {
        yield return new WaitForSeconds(tickTime);
        recieveTickDamage(_damage, tickTime);
    }

    IEnumerator stopTickDamage(float time)
    {
        yield return new WaitForSeconds(time);
        stopDamage();
    }


    private void backToNormal()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }


    public void stopWalking(bool isHook)
    {
        canWalk = false;
        if (!isHook)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    public void keepWalking()
    {
        canWalk = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

}
