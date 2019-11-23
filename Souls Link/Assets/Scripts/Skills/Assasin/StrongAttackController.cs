using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackController : MonoBehaviour
{

    public float damageToEnemies = 20;
    public float stunDuration = 2;
    private bool canStun = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damageToEnemies);
            if (canStun)
            {
                collision.gameObject.GetComponent<SimpleEnemyController>().GetStunned(stunDuration);
            }
        }
    }

    public void setDirection(Vector2 direction, float chargePercent)
    {
        if(chargePercent > 80)
        {
            canStun = true;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, rot);
    }
}
