using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDartController : MonoBehaviour
{
    private float _damage = 0;

    public void setDart(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().startPoison(3);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveTickDamage(_damage, 0.5f);         
            
            Destroy(gameObject);
        }
    }
}
