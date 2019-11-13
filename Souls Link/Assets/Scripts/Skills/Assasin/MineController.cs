using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    private float _tornadoLifeTime = 0;
    private float _damage = 0;
    private GameObject _tornado;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking();
        }
    }

    public void setBomb(float tornadoLifeTime, float damage, GameObject tornado)
    {
        _tornadoLifeTime = tornadoLifeTime;
        _damage = damage;
        _tornado = tornado;
    }
}
