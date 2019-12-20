using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Skill
{
    [SerializeField] private GameObject _hookPrefab;
    [SerializeField] private GameObject _bitePrefab;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _travelTime;
    [SerializeField] private GameObject _linePrefab;
    private bool isShooting = false;
    private GameObject hookObject;
    private PlayerAiming _aiming;
    [HideInInspector]
    public bool canBite = false;
    [HideInInspector]
    public bool hasBitten = false;
    private float biteTracker;
    private float timeToBite = 0.5f;
    Vector2 direction = Vector2.zero;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
        biteTracker = timeToBite;
    }

    private void Update()
    {

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

        if (canBite)
        {
            biteTracker -= Time.deltaTime;
            if (biteTracker < 0)
            {
                canBite = false;
                biteTracker = timeToBite;
            }
        }

       

        /*
        if (_skillMaster.SkillTrigger.skill1.pressedDown && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
        */
    }

    public void pressKey()
    {
        if (!canBite)
        {
            if (_coolDownTracker <= 0)
            {
                shootHook();
                GetComponentInChildren<Animator>().SetBool("isCasting", true);
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                isShooting = true;
            }
        }
        else
        {
            if (!hasBitten)
            {
                biteEnemy();
            }
        }
    }
    
    private void biteEnemy()
    {
        hasBitten = true;
        
        canBite = false;
        
        biteTracker = timeToBite;
        _coolDownTracker = _coolDown;
        GameObject bite = Instantiate(_bitePrefab, gameObject.transform);
        bite.GetComponentInChildren<BiteController>().setBite(gameObject);
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bite.transform.rotation = Quaternion.Euler(0, 0, rot);
        bite.transform.localPosition = new Vector2(0, 0);
        bite.GetComponentInChildren<BiteController>().gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void setBackToNormal()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }
    
    public void stopShooting()
    {
        shooting = false;
    }

    private bool shooting = false;
    public void startShoot()
    {
        shooting = true;
    }

    public void shootHook()
    {
        //if (_coolDownTracker <= 0)
        //{
        direction = _aiming.AimDirection;
        _coolDownTracker = _coolDown;
        hookObject = Instantiate(_hookPrefab, gameObject.transform);
        GameObject line = Instantiate(_linePrefab, gameObject.transform);
        hookObject.GetComponent<HookControl>().setHook(_damage, _hookSpeed, direction, _travelTime, gameObject, line);     
        float rot2 = Mathf.Atan2(_aiming.AimDirection.y, _aiming.AimDirection.x) * Mathf.Rad2Deg;
        hookObject.transform.rotation = Quaternion.Euler(0, 0, rot2 - 90);


        //}
    }

    
}
