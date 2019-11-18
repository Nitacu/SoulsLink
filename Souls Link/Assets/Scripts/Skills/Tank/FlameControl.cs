using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameControl : MonoBehaviour
{
    private float _damagePerTick = 0;
    private float _timeTick = 0;
    private float _damageTime = 0;

    public void setFlame(float damage, float tick, float totalDamageTime)
    {
        _damagePerTick = damage;
        _timeTick = tick;
        _damageTime = totalDamageTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().timeDamage(_damageTime);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveTickDamage(_damagePerTick,_timeTick);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().stopDamage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
