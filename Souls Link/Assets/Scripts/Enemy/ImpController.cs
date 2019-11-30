using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpController : SimpleEnemyController
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _damage;

    public override void attack(GameObject player)
    {
        if (_isHost())
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            _setRangeAttack(direction);
            createdBullet(direction);
        }
    }

    public override void createdBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(_projectile);
        bullet.transform.position = gameObject.transform.position;

        //rotation      
        LinealProjectile projectile = bullet.GetComponent<LinealProjectile>();
        if (projectile != null)
        {
            projectile._projetileOwner = Projectile.ProjectileOwner.ENEMY;
            projectile.Damage = _damage;
            projectile.setRotation(direction);
            projectile.Velocity = direction;
        }
    }
}
