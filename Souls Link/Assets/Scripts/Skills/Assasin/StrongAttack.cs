using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : MonoBehaviour
{

    [SerializeField] private GameObject _attackPrefab;
    private GameObject _attackReference;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _attackDamage;

    private float _coolDownTracker;
    private Vector2 direction;
    

    public bool stillAttackOption = false;

    // Start is called before the first frame update
    void Start()
    {
        _coolDownTracker = _coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }
       
    }

    public void attack()
    {
        if (_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            

            if (stillAttackOption)
            {
                _attackReference = Instantiate(_attackPrefab, GetComponent<PlayerAiming>().getPosition(), Quaternion.identity);
            }
            else
            {
                _attackReference = Instantiate(_attackPrefab, gameObject.transform);
                _attackReference.transform.localPosition = new Vector2(0, GetComponent<PlayerAiming>().getOffsetY());
                
            }
            _attackReference.GetComponentInChildren<StrongAttackController>().damageToEnemies = _attackDamage;
            getDirection();
            _attackReference.GetComponentInChildren<StrongAttackController>().setDirection(direction, GetComponent<Dash>().chargePercent, gameObject);
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
