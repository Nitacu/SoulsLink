using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _damage = 10;

    [SerializeField] private float _fireRate = 1;
    private float _fireRateTracker = 0;

    private void Start()
    {
        _fireRateTracker = _fireRate;
    }

    private void Update()
    {
        _fireRateTracker += Time.deltaTime;

        if (_fireRateTracker >= _fireRate)
        {
            shotProjectileToTarget();
            _fireRateTracker = 0;
        }
    }

    private void shotProjectileToTarget()
    {
        Vector2 direction = _target.transform.position - transform.position;
        direction.Normalize();

        GameObject mineStake = Instantiate(_projectile);
        mineStake.transform.position = gameObject.transform.position;

        LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
        projectile._projetileOwner = Projectile.ProjectileOwner.ENEMY;
        projectile.Damage = _damage;
        projectile.setRotation(direction);
        projectile.Velocity = direction;
    }
}