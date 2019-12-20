using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StakeAttack : Skill
{
    [SerializeField] private GameObject _stakePrefab;

    [SerializeField] private float _damage = 30;

    [HideInInspector]
    public bool canShoot = true;

    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
    }

    private void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

        if (shooting)
        {
            shotStake();
        }

        /*
        if (_skillMaster.SkillTrigger.skill1.pressedDown && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
        */
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

    public void shotStake()
    {
        if (canShoot)
        {
            if (_coolDownTracker <= 0)
            {
                _coolDownTracker = _coolDown;

                GameObject mineStake = Instantiate(_stakePrefab);
                mineStake.GetComponent<StakeControl>().setStake(_damage);
                mineStake.transform.position = gameObject.transform.position;
                LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
                projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
                projectile.setRotation(_aiming.AimDirection.normalized);
                projectile.Velocity = _aiming.AimDirection.normalized;
            }
        }
    }



}
