using System.Collections;
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
    public ControlSpawnEnemys _controlSpawnEnemys;
    public Animator Anim { get => _anim; set => _anim = value; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();
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
        if (player.GetComponent<PlayerHPControl>() != null)
        {
            player.GetComponent<PlayerHPControl>().recieveDamage(20, gameObject);
        }
        else
        {
            player.GetComponent<SelfDestroy>().loseHealth(20);
        }

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
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (GetComponent<EnemyMeleeMultiplayerController>().isMine())
            _controlSpawnEnemys.spawnRandomEnemy();
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
                GetComponentInChildren<SpriteRenderer>().color = Color.red;
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

    public void GetStunned(float duration)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
       GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        canWalk = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        Invoke("noMoreHypnotized", duration);
    }

    public void Hypnotize(float duration)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        canWalk = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        Invoke("noMoreHypnotized", duration);
    }

    public void noMoreHypnotized()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        canWalk = true;       
    }

}
