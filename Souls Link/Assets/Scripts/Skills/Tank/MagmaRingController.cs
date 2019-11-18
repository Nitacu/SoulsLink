using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaRingController : MonoBehaviour
{
    private float _damage;
    private GameObject _playerReference;
    private bool lastRing = false;
    
    public void setMagma(float damage, GameObject player, bool _last)
    {
        _damage = damage;
        _playerReference = player;
        lastRing = _last;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }
    }

    public void DestroyMyself()
    {
        if (lastRing)
        {
            _playerReference.GetComponent<MagmaRing>().setBackToNormal();
        }
        Destroy(gameObject);
    }
}
