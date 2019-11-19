using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GetComponentInParent<DefensiveBallsControl>().doDamage(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
