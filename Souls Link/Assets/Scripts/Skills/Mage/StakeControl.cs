using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeControl : MonoBehaviour
{
    private float _damage = 0;
    

    public void setStake(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }
    }
}
