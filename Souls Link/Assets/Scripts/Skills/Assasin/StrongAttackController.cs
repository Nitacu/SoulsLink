using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackController : MonoBehaviour
{

    public float damageToEnemies = 20;
    public float stunDuration = 2;
    private bool canStun = false;
    private float electricCost = 45;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(damageToEnemies);
            if (canStun)
            {
                collision.gameObject.GetComponent<SimpleEnemyController>().Stun(stunDuration);
            }
        }
    }

    public void setDirection(Vector2 direction, float chargePercent, GameObject playerReference)
    {
        if(chargePercent >= electricCost)
        {
            canStun = true;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            playerReference.GetComponent<Dash>().consumeChargeBar(electricCost);
        }
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, rot);
    }
}
