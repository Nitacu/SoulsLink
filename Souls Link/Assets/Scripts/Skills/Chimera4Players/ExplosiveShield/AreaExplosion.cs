using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{
    private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }
    }

    public void setAreaExplosion(float damage, float timeToDestroy)
    {
        _damage = damage;

        StartCoroutine(destroyMySelf(timeToDestroy));
    }

    IEnumerator destroyMySelf(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);

        Destroy(gameObject);
    }
}
