using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : Skill
{
    [SerializeField] private GameObject _poisonDartPrefab;
    [SerializeField] private float _damagePerTick = 20;

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
            shootPosionDart();
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

    public void shootPosionDart()
    {
        if (canShoot)
        {
            if (_coolDownTracker <= 0)
            {
                _coolDownTracker = _coolDown;

                GameObject poisonDart = Instantiate(_poisonDartPrefab);
                poisonDart.GetComponent<PoisonDartController>().setDart(_damagePerTick);
                poisonDart.transform.position = gameObject.transform.position;
                LinealProjectile projectile = poisonDart.GetComponent<LinealProjectile>();
                projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
                projectile.setRotation(_aiming.AimDirection.normalized);
                projectile.Velocity = _aiming.AimDirection.normalized;
            }
        }
    }
}
