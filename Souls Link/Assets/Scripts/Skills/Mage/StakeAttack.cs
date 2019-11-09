using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeAttack : MonoBehaviour
{
    [SerializeField] private GameObject _stakePrefab;

    [SerializeField] private float _coolDown;
    private float _coolDownTracker;


    [SerializeField] private KeyCode _inputAttack;

    private AimCursor _aiming;

    private void Start()
    {
        _aiming = GetComponent<AimCursor>();
    }

    private void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


        if (Input.GetKeyDown(_inputAttack) && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
    }

    private void shotStake(Vector2 direction)
    {
        GameObject mineStake = Instantiate(_stakePrefab);
        mineStake.transform.position = gameObject.transform.position;
        LinealProjectile projectile = mineStake.GetComponent<LinealProjectile>();
        projectile._projetileOwner = Projectile.ProjectileOwner.PLAYER;
        projectile.setRotation(_aiming.AimVector.normalized);
        projectile.Velocity = _aiming.AimVector.normalized;
    }

    

}
