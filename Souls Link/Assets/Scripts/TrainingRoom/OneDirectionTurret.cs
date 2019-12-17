using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionTurret : MonoBehaviour
{
    public GameObject _bulletPrefab;
    public float angleToShoot;
    public float fireRate = 0.6f;
    public float _damage = 30;
    public Vector2 direction = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("shootBullet", 1, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void shootBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
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
