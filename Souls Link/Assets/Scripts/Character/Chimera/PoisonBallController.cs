using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBallController : MonoBehaviour
{
    private float _damage = 0;
    public GameObject poisonPitPrefab;
    public float _lifetime = 3f;

    public void setPoisonBall(float damage, float lifetime)
    {
        _damage = damage;
        _lifetime = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {        
            GameObject temp = Instantiate(poisonPitPrefab, gameObject.transform.position, Quaternion.identity);
            //Set PoisonBall
            temp.GetComponent<PoisonPitController>().setPit(_damage, _lifetime);
            Destroy(gameObject);
        }
    }
}
