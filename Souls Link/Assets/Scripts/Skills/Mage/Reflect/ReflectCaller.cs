using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCaller : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        GameObject other = collision.gameObject;
        if (other.GetComponent<Projectile>() != null)
        {
            Debug.Log("Projectile enter");
            Projectile projectile = other.GetComponent<Projectile>();

            if (!projectile.Reflected)
            {
                projectile.reflectMySelf();
            }
        }
    }


}
