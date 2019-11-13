using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackController : MonoBehaviour
{

    public float damageToEnemies = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damageToEnemies);
        }
    }

    public void setDirection(Vector2 direction)
    {
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, rot);
    }
}
