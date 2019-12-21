using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSuckerController : MonoBehaviour
{
    
    private GameObject playerReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerReference.GetComponent<BloodSucker>().dealDamage(collision.gameObject);
        }
    }

    public void setBloodSucker(GameObject _player)
    {
        playerReference = _player;       
    }

}
