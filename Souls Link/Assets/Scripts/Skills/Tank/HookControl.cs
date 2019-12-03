using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    private float damage;
    private float hookSpeed;
    private Vector2 direction;
    private float timeToTravel;
    private bool isTraveling = false;
    private bool collidedWithEnemy = false;
    private GameObject _enemyReference;
    private GameObject _playerReference;
    private GameObject _lineReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemyReference = collision.gameObject;
            if(Vector2.Distance(_playerReference.transform.position,_enemyReference.transform.position) < 1.5f)
            {
                collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damage);
                GetComponentInParent<Hook>().setBackToNormal();
                Destroy(_lineReference);
                Destroy(gameObject);
            }
            else
            {
                _enemyReference.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collidedWithEnemy = true;
                collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damage);
                collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking(true);
                collision.gameObject.transform.parent = gameObject.transform;
                isTraveling = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
           
        }

        if (collision.CompareTag("Player"))
        {
            if (collidedWithEnemy)
            {
                collidedWithEnemy = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (_enemyReference != null)
                {
                    _enemyReference.GetComponent<SimpleEnemyController>().keepWalking();
                    _enemyReference.GetComponent<SimpleEnemyController>().Stun(1f);
                    _enemyReference.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    _enemyReference.gameObject.transform.parent = null;
                }
                GetComponentInParent<Hook>().setBackToNormal();
                GetComponentInParent<Hook>().canBite = true;
                Destroy(_lineReference);
                Destroy(gameObject);
            }
        }
    }

    public void setHook(float _damage, float _speedHook, Vector2 _direction, float time, GameObject player, GameObject line)
    {
        _playerReference = player;
        _lineReference = line;
        damage = _damage;
        hookSpeed = _speedHook;
        direction = _direction;
        timeToTravel = time;
        isTraveling = true;
        StartCoroutine(stopHook(timeToTravel));
        StartCoroutine(destroyHook(timeToTravel));


    }

    IEnumerator stopHook(float timeToStop)
    {
        yield return new WaitForSeconds(timeToStop);       
        isTraveling = false;
    }

    IEnumerator destroyHook(float time)
    {
        yield return new WaitForSeconds(time + 0.3f);
        if (!collidedWithEnemy)
        {
            GetComponentInParent<Hook>().setBackToNormal();
            Destroy(_lineReference);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTraveling)
        {
            GetComponent<Rigidbody2D>().velocity = direction * hookSpeed;
        }

        if (collidedWithEnemy)
        {
            GetComponent<Rigidbody2D>().velocity = -direction * hookSpeed;
            if(_enemyReference!=null)
            _enemyReference.GetComponent<Rigidbody2D>().velocity = -direction * hookSpeed;
        }

        Vector3 newPlayerPos = _playerReference.transform.position;
        Vector3 newHookPos = gameObject.transform.position;
        newPlayerPos.z = -0.1f;
        newHookPos.z = -0.1f;

        _lineReference.GetComponent<LineRenderer>().SetPosition(0, newPlayerPos);
        _lineReference.GetComponent<LineRenderer>().SetPosition(1, newHookPos);
    }
}
