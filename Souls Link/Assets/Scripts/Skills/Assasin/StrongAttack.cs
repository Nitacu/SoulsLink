using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : MonoBehaviour
{

    [SerializeField] private KeyCode _inputSkill;
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

        if (Input.GetKeyDown(_inputSkill) && _coolDownTracker <= 0)
        {
            attack();
            _coolDownTracker = _coolDown;
        }
    }

    private void attack()
    {
        if (stillAttackOption)
        {
            _attackReference = Instantiate(_attackPrefab, gameObject.transform.position, Quaternion.identity);  
        }
        else
        {
            _attackReference = Instantiate(_attackPrefab, gameObject.transform);
            _attackReference.transform.localPosition = Vector3.zero;
        }
        _attackReference.GetComponentInChildren<StrongAttackController>().damageToEnemies = _attackDamage;
        getDirection();
        _attackReference.GetComponentInChildren<StrongAttackController>().setDirection(direction);
        StartCoroutine(destroyAttack(_attackReference, _attackTime));
    }

    private void getDirection()
    {
        direction = new Vector2(Input.GetAxis(GetComponent<PlayerMove>().AxisX), Input.GetAxis(GetComponent<PlayerMove>().AxisY)).normalized;
        if(direction == Vector2.zero)
        {
            direction = GetComponent<AimCursor>().LastVector;
        }
    }

    IEnumerator destroyAttack(GameObject attack, float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        Destroy(attack);
    }
}
