using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPitController : MonoBehaviour
{
    float _lifetime = 3f;
    float _damagePerTick = 20f;

    public void setPit(float _damage, float lifetime)
    {
        _damagePerTick = _damage;
        _lifetime = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().startPoison(10);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveTickDamage(_damagePerTick, 0.5f);           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().stopPoisonInstantly();
        }
    }
}
