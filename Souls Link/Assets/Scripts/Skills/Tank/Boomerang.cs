using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Skill
{
    [SerializeField] private GameObject _boomerangPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;


    private bool isShooting = false;
    private float _coolDownTracker;
    private GameObject flame;
    private PlayerAiming _aiming;
    [HideInInspector]
    public bool hasBoomerang = true;

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
        if (_coolDownTracker <= 0)
        {
            if (hasBoomerang)
            {
                spawnFire();
                isShooting = true;
            }
        }
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

    public void spawnFire()
    {
        //if (_coolDownTracker <= 0)
        //{
        hasBoomerang = false;
        Vector2 direction = _aiming.AimDirection;
        _coolDownTracker = _coolDown;
        flame = Instantiate(_boomerangPrefab, gameObject.transform.position, Quaternion.identity);
        flame.GetComponent<BoomerangControl>().setBoomerang(gameObject, _damage, _speed, direction);               

        //}
    }

   
}
