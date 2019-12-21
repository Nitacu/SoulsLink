using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShield : Skill
{
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _shieldExplosion;
    [SerializeField] private float _minDamage = 50f;
    [SerializeField] private float _maxDamage = 200f;
    [SerializeField] private float _shieldLife = 50;
    [SerializeField] private float _shieldLifeTime = 3f;
    [SerializeField] private float _attackTime = 0.2f;



    private GameObject _currentExplosiveShield;

    private void Update()
    {
        if (coolDownTracker <= _coolDown && coolDownTracker > 0)
        {
            coolDownTracker -= Time.deltaTime;
        }
    }

    public void pressKey()
    {
        if (coolDownTracker <= 0)
        {
            coolDownTracker = _coolDown;

            destroyCurrentShield();

            GameObject shield = Instantiate(_shield, gameObject.transform);
            shield.GetComponent<ExplosiveShieldManager>().setShield(_minDamage, _maxDamage, _shieldLife, _shieldLifeTime, _attackTime,_shieldExplosion,gameObject);
            _currentExplosiveShield = shield;
        }
    }

    public void destroyCurrentShield()
    {
        if (_currentExplosiveShield != null)
        {
            _currentExplosiveShield.GetComponent<ExplosiveShield>().StopAllCoroutines();
            Destroy(_currentExplosiveShield);
            _currentExplosiveShield = null;
        }
    }

    public void withShieldMode()
    {
        if (GetComponent<PlayerHPControl>())
        {
            PlayerHPControl playerHP = GetComponent<PlayerHPControl>();

            playerHP.setAbsorbShield(true);
        }
    }

    public void withoutShieldMode()
    {
        if (GetComponent<PlayerHPControl>())
        {
            PlayerHPControl playerHP = GetComponent<PlayerHPControl>();

            playerHP.setAbsorbShield(false);
        }
    }

    public void absorbDamage(float damage)
    {
        if (_currentExplosiveShield != null)
        {
            _currentExplosiveShield.GetComponent<ExplosiveShieldManager>().receiveDamage(damage);
        }
    }
}

