using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public float health = 0;
    public bool canWalk = true;
    private bool facingLeft = false;
    private Rigidbody2D _rb;
    private bool firstTimePressing = true;
    private bool isGettingDamaged = false;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetBool("Can_Walk", canWalk);
    }

    public void attack(GameObject player)
    {
        _anim.Play(Animator.StringToHash("Attack"));
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

    public void stopDamage()
    {
        isGettingDamaged = false;
    }

    public void timeDamage(float time)
    {
        isGettingDamaged = true;
        StartCoroutine(stopTickDamage(time));
    }

    public void recieveTickDamage(float damage, float tickTime)
    {
        if (isGettingDamaged)
        {
            health -= damage;
            if (health < 0)
            {
                Destroy(gameObject);
            }
            StartCoroutine(recieveTick(damage, tickTime));
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("backToNormal", tickTime/2);
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
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void stopWalking()
    {
        canWalk = false;
    }

}
