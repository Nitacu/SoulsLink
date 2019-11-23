using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public float health;
    public bool canWalk = true;
    private bool facingLeft = false;
    private Rigidbody2D _rb;
    private bool firstTimePressing = true;
    private bool isGettingDamaged = false;
    private Animator _anim;
    private EnemyMeleeMultiplayerController _multiplayerController;
    public ControlSpawnEnemys _controlSpawnEnemys;
    public Animator Anim { get => _anim; set => _anim = value; }
    private float _force = 0;
    private bool isGettingKnocked = false;
    private Vector2 _knockBackDirection;

    // Start is called before the first frame update
    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();
        _multiplayerController = GetComponent<EnemyMeleeMultiplayerController>();
    }

    // Update is called once per frame
    public void Update()
    {
        Anim.SetBool("Can_Walk", canWalk);

        if (isGettingKnocked)
        {
            _rb.velocity = _knockBackDirection * _force;
        }
    }

    public void changeOrientation(float value)
    {
        if (value > 0)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = true;
    }

    public virtual void attack(GameObject player)
    {
        //solo el host hace lo del daño 
        if (_multiplayerController.isHost())
        {
            //envia a los demas la informacion para que se vea que ataco
            _multiplayerController.setAttack();

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
    }

    public void recieveDamage(float damage)
    {
        if (_multiplayerController.isHost())
        {
            health -= damage;
            _multiplayerController.changeHealth(health);
            if (health < 0)
            {
                StartCoroutine(die());
            }
            else
            {
                StartCoroutine(changeColor(0.5f));
            }
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

    public IEnumerator die()
    {
        Anim.Play(Animator.StringToHash("Death"));
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (GetComponent<EnemyMeleeMultiplayerController>().isHost())
            _controlSpawnEnemys.spawnRandomEnemy();
    }

    public void recieveTickDamage(float damage, float tickTime)
    {
        if (_multiplayerController.isHost())
        {
            if (isGettingDamaged)
            {
                health -= damage;
                _multiplayerController.changeHealth(health);

                if (health < 0)
                {
                    StartCoroutine(die());
                }
                else
                {
                    StartCoroutine(recieveTick(damage, tickTime));
                    StartCoroutine(changeColor(0.5f));
                }
            }
            Debug.Log(health);
        }
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


    public IEnumerator changeColor(float interval)
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(interval);
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

    public void getKnocked(float force, float damage, float duration, Vector2 knockBackDirection)
    {
        _force = force;

        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        _knockBackDirection = knockBackDirection;
        isGettingKnocked = true;
        StartCoroutine(stopKnockBack(duration));
    }

    IEnumerator stopKnockBack(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        _rb.velocity = Vector2.zero;
        isGettingKnocked = false;
    }
}
