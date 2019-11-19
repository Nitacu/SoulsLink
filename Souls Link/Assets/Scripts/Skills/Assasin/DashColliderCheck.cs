using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashColliderCheck : MonoBehaviour
{
    public float damage = 20;
    public float stunDuration = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damage);
        }
    }

    public void setStun(float _stunDurationn)
    {
        stunDuration = _stunDurationn;
    }

    public void setDamage(float _damage)
    {
        damage = _damage;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().GetStunned(stunDuration);
        }
    }

}
