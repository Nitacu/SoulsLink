using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float _duration = 6;
    public float bulletDamage = 15;
    public float _fireRate = 0.4f;
    public GameObject stakePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void startShooting()
    {
        shootStake();
        StartCoroutine(destroyMachineGun(_duration));
    }

    public void shootStake()
    {
        GameObject mineStake = Instantiate(stakePrefab);
        GameObject secondStake = Instantiate(stakePrefab);
        GameObject thirdStake = Instantiate(stakePrefab);

        mineStake.GetComponent<StakeControl>().setStake(bulletDamage);
        mineStake.transform.position = gameObject.transform.position;
        LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        projectile.setRotation(GetComponentInParent<PlayerAiming>().AimDirection.normalized);
        projectile.Velocity = GetComponentInParent<PlayerAiming>().AimDirection.normalized;

        secondStake.GetComponent<StakeControl>().setStake(bulletDamage);
        secondStake.transform.position = gameObject.transform.position;
        LinealProjectile secondProjectile = secondStake.GetComponent<LinealProjectile>();
        secondProjectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        float theRotation = Mathf.Atan2(GetComponentInParent<PlayerAiming>().AimDirection.y, GetComponentInParent<PlayerAiming>().AimDirection.x) * Mathf.Rad2Deg;
        theRotation += 20;
        Vector2 newDirectionPositive = new Vector2(Mathf.Cos(theRotation*Mathf.Deg2Rad), Mathf.Sin(theRotation * Mathf.Deg2Rad));
        secondProjectile.setRotation(newDirectionPositive.normalized);
        secondProjectile.Velocity = newDirectionPositive.normalized;
       

        thirdStake.GetComponent<StakeControl>().setStake(bulletDamage);
        thirdStake.transform.position = gameObject.transform.position;
        LinealProjectile thirdProjectile = thirdStake.GetComponent<LinealProjectile>();
        thirdProjectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        float the2Rotation = Mathf.Atan2(GetComponentInParent<PlayerAiming>().AimDirection.y, GetComponentInParent<PlayerAiming>().AimDirection.x) * Mathf.Rad2Deg;
        the2Rotation -= 20;
        Vector2 newDirectionNegative = new Vector2(Mathf.Cos(the2Rotation * Mathf.Deg2Rad), Mathf.Sin(the2Rotation * Mathf.Deg2Rad));
        thirdProjectile.setRotation(newDirectionNegative.normalized);
        thirdProjectile.Velocity = newDirectionNegative.normalized;
        




        StartCoroutine(fire(_fireRate));
    }

    IEnumerator fire(float timeToShoot)
    {
        yield return new WaitForSeconds(timeToShoot);
        shootStake();
    }

    IEnumerator destroyMachineGun(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        float rot = Mathf.Atan2(GetComponentInParent<PlayerAiming>().AimDirection.y, GetComponentInParent<PlayerAiming>().AimDirection.x) * Mathf.Rad2Deg;
        if (Mathf.Sign(GetComponentInParent<PlayerAiming>().AimDirection.x) < 0)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rot);
    }
}
