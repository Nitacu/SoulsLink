using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBall : Skill
{
    [SerializeField] private GameObject _poisonBallPrefab;
    [SerializeField] private float _coolDown = 1.5f;
    [SerializeField] private float _damagePerTick = 20;
    [SerializeField] private float pitLifetime = 3f;
    private float _coolDownTracker;

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
            shootPoisonBall();
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

    public void shootPoisonBall()
    {
        if (canShoot)
        {
            if (_coolDownTracker <= 0)
            {
                _coolDownTracker = _coolDown;

                GameObject poisonDart = Instantiate(_poisonBallPrefab);
                poisonDart.GetComponent<PoisonBallController>().setPoisonBall(_damagePerTick, pitLifetime);
                poisonDart.transform.position = gameObject.transform.position;
                LinealProjectile projectile = poisonDart.GetComponent<LinealProjectile>();
                projectile.Damage = _damagePerTick/2;
                projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
                projectile.setRotation(_aiming.AimDirection.normalized);
                projectile.Velocity = _aiming.AimDirection.normalized;
            }
        }
    }
}
