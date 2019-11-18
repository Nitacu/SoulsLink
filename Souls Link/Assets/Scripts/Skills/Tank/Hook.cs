using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject _hookPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _travelTime;
    [SerializeField] private GameObject _linePrefab;
    private bool isShooting = false;
    private float _coolDownTracker;
    private GameObject hookObject;
    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
    }

    private void Update()
    {

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
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
        if(_coolDownTracker <= 0)
        {
            shootHook();
            GetComponentInChildren<Animator>().SetBool("isCasting", true);
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            isShooting = true;
        }
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
        Vector2 direction = _aiming.AimDirection;
        _coolDownTracker = _coolDown;
        hookObject = Instantiate(_hookPrefab, gameObject.transform);
        GameObject line = Instantiate(_linePrefab, gameObject.transform);
        hookObject.GetComponent<HookControl>().setHook(_damage, _hookSpeed, direction, _travelTime, gameObject, line);     
        float rot2 = Mathf.Atan2(_aiming.AimDirection.y, _aiming.AimDirection.x) * Mathf.Rad2Deg;
        hookObject.transform.rotation = Quaternion.Euler(0, 0, rot2 - 90);


        //}
    }

    
}
