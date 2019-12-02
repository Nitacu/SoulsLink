using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteController : MonoBehaviour
{
    public float _damage = 20f;
    public float _heal = 20f;
    GameObject playerReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            playerReference.GetComponent<PlayerHPControl>().healHP(_heal);
            Debug.Log("LE PEGUÉ AL ENEMIGO CON EL MORDISCO");
        }
    }

    public void setBite(GameObject player)
    {
        playerReference = player;
    }

    public void DestroyMyself()
    {
        playerReference.GetComponent<Hook>().hasBitten = false;
        Destroy(gameObject);
    }
}
