﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : Skill
{

    [SerializeField] private GameObject _attackPrefab;
    private GameObject _attackReference;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _attackDamage;

    [SerializeField] private bool _alwaysStrong = false;

    private Vector2 direction;


    public bool stillAttackOption = false;

    // Start is called before the first frame update
    void Start()
    {
        CoolDownTracker = _coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }

    }

    public void attack()
    {
        if (CoolDownTracker <= 0)
        {
            CoolDownTracker = _coolDown;

            if (stillAttackOption)
            {
                _attackReference = Instantiate(_attackPrefab, GetComponent<PlayerAiming>().getPosition(), Quaternion.identity);
            }
            else
            {
                _attackReference = Instantiate(_attackPrefab, gameObject.transform);
                _attackReference.transform.localPosition = new Vector2(0, GetComponent<PlayerAiming>().getOffsetY());

            }

            if (GetComponent<Mist>())
            {
                if (GetComponent<Mist>().IsStealth)
                {
                    _attackReference.GetComponentInChildren<StrongAttackController>().damageToEnemies = _attackDamage * 2;
                }
                else
                {
                    _attackReference.GetComponentInChildren<StrongAttackController>().damageToEnemies = _attackDamage;
                }
            }

            getDirection();

            _attackReference.GetComponentInChildren<StrongAttackController>().setDirection(direction, (_alwaysStrong) ? 100 : GetComponent<Dash>().chargePercent, gameObject);
            StartCoroutine(destroyAttack(_attackReference, _attackTime));
        }
    }



    private void getDirection()
    {
        direction = GetComponent<PlayerAiming>().AimDirection.normalized;
    }

    IEnumerator destroyAttack(GameObject attack, float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        Destroy(attack);
    }
}
