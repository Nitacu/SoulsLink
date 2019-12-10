using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangControl : MonoBehaviour
{
    private float _damage = 20f;
    private GameObject _playerReference;
    private float _speed;
    private float _timeToComeBack = 0.4f;
    private Rigidbody2D _rb;
    private bool isReturning = false;
    private Vector2 _movingDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = gameObject.transform.rotation.eulerAngles;
        temp.z = temp.z + 4;
        gameObject.transform.rotation = Quaternion.Euler(temp);

        if (!isReturning)
        {
            GetComponent<Rigidbody2D>().velocity = _movingDirection * _speed;
            Debug.Log("hola soy un marranito");
        }
        else
        {
            float step = _speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, _playerReference.transform.position, step);
            Debug.Log("Hola soy un loro");
        }

        if(gameObject.transform.position == _playerReference.transform.position && isReturning)
        {
            _playerReference.GetComponent<Boomerang>().hasBoomerang = true;
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }

        if (isReturning)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject == _playerReference)
                {
                    _playerReference.GetComponent<Boomerang>().hasBoomerang = true;
                    Destroy(gameObject);
                }
            }
        }

        if (collision.CompareTag("Wall"))
        {
            isReturning = true;
            _rb.velocity = Vector2.zero;
        }

    }

    public void setBoomerang(GameObject _player, float damage , float speed, Vector2 direction)
    {
        _playerReference = _player;
        _damage = damage;
        _speed = speed;
        _movingDirection = direction;
        StartCoroutine(makeBoomerangReturn(_timeToComeBack));
    }

    IEnumerator makeBoomerangReturn(float time)
    {
        yield return new WaitForSeconds(time);
        isReturning = true;
        _rb.velocity = Vector2.zero;
    }
}
