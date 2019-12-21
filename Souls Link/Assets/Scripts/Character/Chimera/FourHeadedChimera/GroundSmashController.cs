using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmashController : MonoBehaviour
{
    public float _damage = 100;
    private GameObject playerReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }
    }

    public void setGroundSmash(float damage, GameObject _player)
    {
        playerReference = _player;
        _damage = damage;
    }

    private void destoyMyself()
    {
        playerReference.GetComponent<GroundSmash>().backToNormal();
        Destroy(gameObject);
    }
}
