using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaRingController : MonoBehaviour
{
    private float _damage;
    private GameObject _playerReference;
    private bool lastRing = false;
    private float _time;
    private float _heal = 20f;
    
    public void setMagma(float damage, GameObject player, bool _last, float timeAlive)
    {
        _damage = damage;
        _playerReference = player;
        lastRing = _last;
        _time = timeAlive;
        _heal = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().timeDamage(10);
            _playerReference.GetComponent<PlayerHPControl>().healHP(_heal);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveTickDamage(_damage, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().stopDamage();
        }
    }

    public void DestroyMyself()
    {
        StartCoroutine(destroyRing(_time));
    }

    IEnumerator destroyRing(float time)
    {
        //Time for the ring to get destroyed
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
