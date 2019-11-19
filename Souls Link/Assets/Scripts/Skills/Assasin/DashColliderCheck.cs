using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashColliderCheck : MonoBehaviour
{
    public float damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damage);
        }
    }

}
