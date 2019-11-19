using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypnosisSkillControl : MonoBehaviour
{
    private float _damage;
    private Vector2 _direction;

    public void setHypnosis(Vector2 direction, float damage)
    {
        _damage = damage;
        _direction = direction;

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //HipnotizeEnemy;
        }
    }
}
