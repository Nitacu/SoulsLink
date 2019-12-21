using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShieldManager : MonoBehaviour
{
    private GameObject _playerReferece;
    private GameObject _shieldExplosion;
    private float _attackTime;

    private float _minDamage;
    private float _maxDamage;
    private float _shieldLife;
    private float _currentShieldLife;
    private float _lifeTime = 10;

    public float _currentDamageAbsorbe;

    public void setShield(float minDamage, float maxDamage, float shieldLife, float lifeTime, float attackTime,GameObject shieldExplosion, GameObject playerRef)
    {
        _minDamage = minDamage;
        _maxDamage = maxDamage;
        _shieldLife = shieldLife;
        _currentShieldLife = shieldLife;
        _playerReferece = playerRef;
        _shieldExplosion = shieldExplosion;
        _attackTime = attackTime;
        _currentDamageAbsorbe = _minDamage;
        _lifeTime = lifeTime;
    }

    private void Update()
    {
        if (_lifeTime > 0)
        {
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            DestroyDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void destroyMySelf()
    {
        if (_playerReferece != null)
        {
            _playerReferece.GetComponent<ExplosiveShield>().withoutShieldMode();
        }

        Destroy(gameObject);
    }

    private void DestroyDamage()
    {
        if (_currentShieldLife > 0)
        {
            //calcular daño
            float lifePercentage = _currentShieldLife / _shieldLife;
            float damagePercentage = 1 - lifePercentage;
            float damage = _maxDamage * damagePercentage;

            damage = Mathf.Clamp(damage, _minDamage, _maxDamage);

            //crear area
            GameObject explosionArea = Instantiate(_shieldExplosion, gameObject.transform);
            explosionArea.GetComponent<AreaExplosion>().setAreaExplosion(damage, _attackTime);
        }
    }

    public void receiveDamage(float damage)
    {
        _currentShieldLife -= damage;

        if (_currentShieldLife < 0)
        {
            //morir
            destroyMySelf();
        }
    }

}
