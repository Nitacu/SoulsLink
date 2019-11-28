using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTurret : MonoBehaviour
{
    public GameObject _bullet;
    public Transform _firePoint;
    public float _bulletDamage = 20;
    private Transform _target;
    public float _turretRange = 1f;
    public float _fireRate = 0.5f;
    private float _fireCountdown;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        //Find all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        //Check in enemies which has the closest distance to turret to make him a target
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //set target to enemy
        if(nearestEnemy != null && shortestDistance <= _turretRange)
        {
            _target = nearestEnemy.transform;
        }
        else
        {
            _target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_target == null)
        {
            return;
        }

        Vector2 enemyDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(_fireCountdown <= 0)
        {
            shootBullet();
            _fireCountdown = _fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    private void shootBullet()
    {
        Vector2 direction = _target.transform.position - transform.position;
        direction.Normalize();

        GameObject mineStake = Instantiate(_bullet, _firePoint.position, Quaternion.identity);
        //mineStake.transform.position = gameObject.transform.position;

        LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        projectile.Damage = _bulletDamage;
        projectile.setRotation(direction);
        projectile.Velocity = direction;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _turretRange);
    }
}
