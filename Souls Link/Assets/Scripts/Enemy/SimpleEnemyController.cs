using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SimpleEnemyController : MonoBehaviour
{
    public float health;
    public ElementsEnumScript.CharacterElement _elementType;
    public bool canWalk = true;
    private bool facingLeft = false;
    private Rigidbody2D _rb;
    private bool firstTimePressing = true;
    private bool isGettingDamaged = false;
    private Animator _anim;
    private PolyNav.PolyNavAgent _poly;
    public ControlSpawnEnemys _controlSpawnEnemys;
    public Animator Anim { get => _anim; set => _anim = value; }
    private float _force = 0;
    private bool isGettingKnocked = false;
    private Vector2 _knockBackDirection;
    [SerializeField] private Transform _positionAttack;
    public bool _flip = false;
    public ParticleSystem _poisonParticles;
    private bool isPoisoned = false;
    [HideInInspector]
    public bool isStunned = false;
    private Vector2 tempVelocity = Vector2.zero;
    private float tempMaxSpeed = 0;
    private GameObject tempPlayer;
    public bool isDummy = false;

    #region Delegate
    public delegate bool DelegateEnemyMultiplayerController();
    public DelegateEnemyMultiplayerController _isMine;
    public DelegateEnemyMultiplayerController _isHost;
    public delegate void DelegateEnemyMultiplayerControllerHealth(float health);
    public DelegateEnemyMultiplayerControllerHealth _changeHealth;
    public delegate void DelegateEnemyMultiplayerControllerAttack();
    public DelegateEnemyMultiplayerControllerAttack _setAttack;
    public delegate void DelegateEnemyMultiplayerControllerRangeAttack(Vector2 direction);
    public DelegateEnemyMultiplayerControllerRangeAttack _setRangeAttack;
    public delegate void DelegateEnemyMultiplayerControllerDestroy();
    public DelegateEnemyMultiplayerControllerDestroy _destroySelf;
    public delegate void DelegateEnemyMultiplayerControllerDestroyFlip(bool flip);
    public DelegateEnemyMultiplayerControllerDestroyFlip _setFlip;
    #endregion

    // Start is called before the first frame update
    public void Start()
    {
        if (!isDummy)
        {
            _rb = GetComponent<Rigidbody2D>();
            Anim = GetComponentInChildren<Animator>();
            _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();
            _poly = GetComponent<PolyNav.PolyNavAgent>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        

        if (isGettingKnocked)
        {
            _rb.velocity = _knockBackDirection * _force;
        }

        if (!isDummy)
        {
            Anim.SetBool("Can_Walk", canWalk);

            if (_flip)
            {
                _anim.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                _anim.transform.localScale = new Vector3(1, 1, 1);
            }

            if (_poly.enabled)
            {
                changeOrientation(_poly.velocity.x);
            }
        }
    }

    public void changeOrientation(float value)
    {
        bool aux = _flip;
        if (value > 0)
        {
            _flip = false;
        }
        else
        {
            _flip = true;
        }

        if (_isHost() && aux != _flip)
            _setFlip(_flip);
    }


    // no borrar
    public virtual void createdBullet(Vector2 direction)
    {

    }

    public virtual void attack(GameObject player)
    {
        tempPlayer = player;
        if (!isStunned)
        {
            //solo el host hace lo del daño 
            if (_isHost())
            {
                //envia a los demas la informacion para que se vea que ataco
                _setAttack();

                Anim.Play(Animator.StringToHash("Attack"));
            }
        }
    }

    public void checkIfDamages()
    {
        if (_isHost())
        {
            if (Vector2.Distance(transform.position, tempPlayer.transform.position) <= 1)
            {
                exeAttack();
            }
            tempPlayer = null;
        }
    }

    public void exeAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_positionAttack.position, new Vector2(1.19f, 2.59f), 0);

        if (colliders.Length > 0)
        {
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    if (col.GetComponent<PlayerHPControl>() != null)
                    {
                        col.GetComponent<PlayerHPControl>().recieveDamage(20, gameObject);
                    }
                    else
                    {
                        col.GetComponent<SelfDestroy>().loseHealth(20);
                    }

                }
            }
        }
    }

    public void recieveDamage(float damage)
    {
        Debug.Log("RECEIVE DAMAGE SIMPLE ENEMY");

        if (_isHost())
        {
            if (!isDummy)
            {
                health -= damage;
                _changeHealth(health);
                if (health < 0)
                {
                    StartCoroutine(die());
                }
                else
                {
                    StartCoroutine(changeColor(0.5f));
                }
            }
            else
            {
                GetComponent<PhotonEnemyMultiplayerController>().setKill();
                GetComponent<DummyController>().killDummy();
            }
        }
    }

    public void stopDamage()
    {
        isGettingDamaged = false;
    }

    public void stopPoisonInstantly()
    {
        StartCoroutine(stopPoison(0.1f));
    }

    public void timeDamage(float time)
    {
        isGettingDamaged = true;
        StartCoroutine(stopTickDamage(time));
    }

    public void startPoison(float time)
    {
        isPoisoned = true;
        _poisonParticles.Play();
        isGettingDamaged = true;

        StartCoroutine(stopPoison(time));
    }

    public IEnumerator die()
    {
        Debug.Log("CORRUTINA DIE SIMPLE ENEMEY");
        Anim.Play(Animator.StringToHash("Death"));
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;
        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            GetComponent<PolyNav.PolyNavAgent>().enabled = false;
        }
        GetComponent<BehaviorTree>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        if (_isHost())
        {
            _destroySelf();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void recieveTickDamage(float damage, float tickTime)
    {
        if (_isHost())
        {
            if (!isDummy)
            {
                if (isGettingDamaged)
                {
                    health -= damage;
                    _changeHealth(health);

                    if (health < 0)
                    {
                        StartCoroutine(die());
                    }
                    else
                    {
                        StartCoroutine(recieveTick(damage, tickTime));
                        if (!isPoisoned)
                        {
                            changeRedToWhiteColor(0.3f);
                        }
                        else
                        {
                            changePurpleToWhiteColor(0.3f);
                        }
                    }
                }
                Debug.Log(health);
            }
            else
            {
                GetComponent<PhotonEnemyMultiplayerController>().setKill();
                GetComponent<DummyController>().killDummy();
            }
        }
    }

    IEnumerator stopPoison(float time)
    {
        yield return new WaitForSeconds(time);
        _poisonParticles.Stop();
        isGettingDamaged = false;
        isPoisoned = false;
    }

    private void changePurpleToWhiteColor(float time)
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        Invoke("changeToWhite", time);
    }

    private void changeRedToWhiteColor(float time)
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        Invoke("changeToWhite", time);
    }

    private void changeToWhite()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
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
        if (!isStunned)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
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
        isStunned = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            GetComponent<PolyNav.PolyNavAgent>().isStunned = true;
        }
        Invoke("noMoreStun", duration);
    }

    public void GetRooted(float duration)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        canWalk = false;
        isStunned = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            GetComponent<PolyNav.PolyNavAgent>().isStunned = true;
        }
        Invoke("noMoreStun", duration);
    }

    public void Stun(float duration)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        canWalk = false;
        isStunned = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.cyan;

        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            tempMaxSpeed = GetComponent<PolyNav.PolyNavAgent>().maxSpeed;
            tempVelocity = GetComponent<PolyNav.PolyNavAgent>().velocity;
            GetComponent<PolyNav.PolyNavAgent>().maxSpeed = 0;
            GetComponent<PolyNav.PolyNavAgent>().velocity = Vector2.zero;
            GetComponent<PolyNav.PolyNavAgent>().isStunned = true;
        }
        //StartCoroutine(stopTheStun(duration));
        Invoke("noMoreStun", duration);
    }

    IEnumerator stopTheStun(float time)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        canWalk = true;
        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            GetComponent<PolyNav.PolyNavAgent>().maxSpeed = tempMaxSpeed;
            GetComponent<PolyNav.PolyNavAgent>().velocity = tempVelocity;
            isStunned = false;
            GetComponent<PolyNav.PolyNavAgent>().isStunned = false;
        }
    }

    public void noMoreStun()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        if (GetComponent<PolyNav.PolyNavAgent>() != null)
        {
            GetComponent<PolyNav.PolyNavAgent>().maxSpeed = tempMaxSpeed;
            GetComponent<PolyNav.PolyNavAgent>().velocity = tempVelocity;
            GetComponent<PolyNav.PolyNavAgent>().isStunned = false;
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        canWalk = true;
        isStunned = false;
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

    //This method makes all the operations according to increase or decrease the damage that the object will recieve
    private float calculateDamageToEffect(float damage, ElementsEnumScript.SkillElements _damageElement)
    {
        return damage;
    }
}
