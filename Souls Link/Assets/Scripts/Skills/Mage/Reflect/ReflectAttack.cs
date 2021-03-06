﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAttack : Skill
{

    [SerializeField] private float _radiusDetection;
    [SerializeField] private float _coneDetectioneDistance;

    [SerializeField] private GameObject _shielAroundPrefab;
    [SerializeField] private GameObject _shieldDirectionPrefab;
    private GameObject _shieldReference;

    [SerializeField] private float _reflectingTime;
    private float _reflectingTimeTracker = 0;
    private bool reflecting;

    enum ReflectType
    {
        AROUND,
        DIRECTION
    }

    [SerializeField] ReflectType _reflectType;

    private PlayerAiming _aiming;
    private bool buttonPressed = false;

    private void Start()
    {
        _aiming = gameObject.GetComponent<PlayerAiming>();
    }

    public void pressKey()
    {
        if(CoolDownTracker <= 0)
            buttonPressed = true;
    }

    private void stopMoving()
    {
        GetComponent<PlayerHPControl>().setReflectiveMode();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
        GetComponent<StakeAttack>().canShoot = false;
    }

    private void backToNormal()
    {
        GetComponent<PlayerHPControl>().setNormalMode();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
        GetComponent<StakeAttack>().canShoot = true;
    }

    private void Update()
    {
        if (buttonPressed && !reflecting && CoolDownTracker <= 0)
        {
            reflecting = true;
            
            _reflectingTimeTracker = _reflectingTime;

            //feedback start
            switch (_reflectType)
            {
                case ReflectType.AROUND:
                    GetComponent<PlayerHPControl>().setReflectiveMode();
                    GetComponent<StakeAttack>().canShoot = false;
                    _shieldReference = Instantiate(_shielAroundPrefab, gameObject.transform);
                    break;
                case ReflectType.DIRECTION:
                    _shieldReference = Instantiate(_shieldDirectionPrefab, gameObject.transform);
                    _shieldReference.GetComponent<RotateAroundByAim>().rotateAround(GetComponent<PlayerAiming>().AimDirection);
                    break;
            }
            CoolDownTracker = _coolDown;
        }

        if (reflecting)
        {
            if (_reflectingTimeTracker > 0)
            {
                _reflectingTimeTracker -= Time.deltaTime;

                switch (_reflectType)
                {
                    case ReflectType.AROUND:
                        detectProjectilesAround();
                        break;
                    case ReflectType.DIRECTION:
                        detectoProjectilesOnDirection();
                        break;
                }
            }
            else
            {
                reflecting = false;
                buttonPressed = false;
                
                //feedback end
                Destroy(_shieldReference);
                GetComponent<StakeAttack>().canShoot = true;
                GetComponent<PlayerHPControl>().setNormalMode();
            }

        }

        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
    }

    private void detectoProjectilesOnDirection()
    {
        //Debug.Log("Direction: " + _aiming.AimDirection * 0.1f);
        Vector2 offset = (Vector2)transform.position + _aiming.AimDirection * 0.5f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(offset, 0.3f, _aiming.AimDirection, _coneDetectioneDistance);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                GameObject other = hit.collider.gameObject;
                if (other.GetComponent<Projectile>() != null)
                {
                    Debug.Log("Projectile enter");
                    Projectile projectile = other.GetComponent<Projectile>();

                    if (!projectile.Reflected)
                    {
                        projectile.reflectMySelf();
                        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
                    }
                }
            }
        }
    }

    private void detectProjectilesAround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radiusDetection);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                GameObject other = hit.gameObject;
                if (other.GetComponent<Projectile>() != null)
                {
                    Debug.Log("Projectile enter");
                    Projectile projectile = other.GetComponent<Projectile>();

                    if (!projectile.Reflected)
                    {
                        projectile.reflectMySelf();
                        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Color newColor = Color.blue;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, _radiusDetection);
    }

}
