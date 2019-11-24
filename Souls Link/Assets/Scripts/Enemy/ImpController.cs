using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpController : SimpleEnemyController
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _damage;

    public override void attack(GameObject player)
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

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

        /*
        GameObject aux = Instantiate(_projectile,transform.position,Quaternion.identity);

        angle = Vector2.Angle(transform.position,player.transform.position);
        Debug.Log(angle *Mathf.Rad2Deg);

        aux.transform.localEulerAngles = new Vector3(0,0,angle);
        */
    }
}
