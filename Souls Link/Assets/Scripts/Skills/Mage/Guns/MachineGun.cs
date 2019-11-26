using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float _duration = 5;
    public float bulletDamage = 10;
    public float _fireRate = 0.2f;
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
        mineStake.GetComponent<StakeControl>().setStake(bulletDamage);
        mineStake.transform.position = gameObject.transform.position;
        LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        projectile.setRotation(GetComponentInParent<PlayerAiming>().AimDirection.normalized);
        projectile.Velocity = GetComponentInParent<PlayerAiming>().AimDirection.normalized;

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
