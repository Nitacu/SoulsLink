﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Skill
{
    #region BEHAVIOUR
    public enum FollowingRotation
    {
        LASTDIRECTION,
        FOLLOWDIRECTION
    }
    [SerializeField] private FollowingRotation _followingRotation;
    #endregion

    [SerializeField] private GameObject _flameThrowerParticles;
    [SerializeField] private GameObject _colliderFire;
    [SerializeField] private float flameDuration;
    [SerializeField] private float _damagePerTick;
    [SerializeField] private float _timeTicks;

    private bool isShooting = false;
    private float _coolDownTracker;
    private GameObject flame;
    private GameObject colliderFlame;
    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = 0;
    }

    private void Update()
    {

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

        if (_followingRotation == FollowingRotation.FOLLOWDIRECTION)
        {
            if (isShooting)
            {
                float rot = Mathf.Atan2(_aiming.AimDirection.x, _aiming.AimDirection.y) * Mathf.Rad2Deg;
                float rot2 = Mathf.Atan2(_aiming.AimDirection.y, _aiming.AimDirection.x) * Mathf.Rad2Deg;
                flame.transform.rotation = Quaternion.Euler(0, 0, rot2);
                colliderFlame.transform.rotation = Quaternion.Euler(0, 0, rot2);
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
        if (_coolDownTracker <= 0)
        {
            spawnFire();
            isShooting = true;
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
        Vector2 direction = _aiming.AimDirection;
        _coolDownTracker = _coolDown;
        flame = Instantiate(_flameThrowerParticles, gameObject.transform);
        colliderFlame = Instantiate(_colliderFire, gameObject.transform);
        colliderFlame.GetComponent<FlameControl>().setFlame(_damagePerTick, _timeTicks, flameDuration);
        StartCoroutine(destroyFire(flame, flameDuration, colliderFlame));
        flame.transform.position = gameObject.transform.position;

        float rot = Mathf.Atan2(_aiming.AimDirection.y, _aiming.AimDirection.x) * Mathf.Rad2Deg;
        flame.transform.rotation = Quaternion.Euler(0, 0, rot);
        float rot2 = Mathf.Atan2(_aiming.AimDirection.y, _aiming.AimDirection.x) * Mathf.Rad2Deg;
        colliderFlame.transform.rotation = Quaternion.Euler(0, 0, rot2);

        //}
    }

    IEnumerator destroyFire(GameObject fire, float time, GameObject colliderF)
    {
        yield return new WaitForSeconds(time);
        isShooting = false;
        Destroy(colliderF);
        Destroy(fire);
    }
}
