using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Skill
{
    [SerializeField] private GameObject _boomerangPrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;


    private bool isShooting = false;
    private GameObject flame;
    private PlayerAiming _aiming;
    [HideInInspector]
    public bool hasBoomerang = true;
    private bool hitWall = false;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
    }

    private void Update()
    {


        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


        /*
        if (_skillMaster.SkillTrigger.skill1.pressedDown && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
        */
    }

    public void pressKey()
    {
        if (_coolDownTracker <= 0)
        {
            if (hasBoomerang)
            {
                spawnFire();
                isShooting = true;
            }
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

    public void spawnFire()
    {
        //if (_coolDownTracker <= 0)
        //{
        Vector2 direction = _aiming.AimDirection;
        //Raycast
        checkRays(direction);

        if (!hitWall)
        {
            hasBoomerang = false;
            _coolDownTracker = _coolDown;
            flame = Instantiate(_boomerangPrefab, gameObject.transform.position, Quaternion.identity);
            flame.GetComponent<BoomerangControl>().setBoomerang(gameObject, _damage, _speed, direction);
        }
        else
        {
            hitWall = false;
        }


                    

        //}
    }

    private void checkRays(Vector2 direction)
    {

        Vector3 position2 = gameObject.transform.position;
        position2.x += 0.5f;
        Vector3 position3 = gameObject.transform.position;
        position3.x -= 0.5f;

        //Raycast
        RaycastHit2D[] ray = Physics2D.RaycastAll(gameObject.transform.position, direction, 1.08f);
        RaycastHit2D[] ray2 = Physics2D.RaycastAll(position2, direction, 1.08f);
        RaycastHit2D[] ray3 = Physics2D.RaycastAll(position3, direction, 1.08f);

        Debug.DrawRay(gameObject.transform.position, direction * 1.08f, Color.red);
        Debug.DrawRay(position2, direction * 1.08f, Color.red);
        Debug.DrawRay(position3, direction * 1.08f, Color.red);

        foreach (RaycastHit2D _ray in ray)
        {
            if (_ray.collider.gameObject.CompareTag("Wall"))
            {
                hitWall = true;
                break;
            }
        }
        foreach (RaycastHit2D _ray in ray2)
        {
            if (_ray.collider.gameObject.CompareTag("Wall"))
            {
                hitWall = true;
                break;
            }
        }
        foreach (RaycastHit2D _ray in ray3)
        {
            if (_ray.collider.gameObject.CompareTag("Wall"))
            {
                hitWall = true;
                break;
            }
        }
    }
   
}
