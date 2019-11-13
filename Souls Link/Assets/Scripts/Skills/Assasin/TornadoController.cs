using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    private GameObject enemyReference;
    
    public void destroyTornado(float lifetime)
    {
        Invoke("destroyMyself", lifetime);
    }

    private void destroyMyself()
    {
        if(enemyReference != null)
        {
            Vector3 temp = enemyReference.gameObject.transform.rotation.eulerAngles;
            temp.z = 0;
            enemyReference.gameObject.transform.rotation = Quaternion.Euler(temp);
            enemyReference.GetComponent<SimpleEnemyController>().canWalk = true;
        }
       
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            enemyReference = collision.gameObject;
            Vector3 temp = collision.gameObject.transform.rotation.eulerAngles;
            temp.z = temp.z + 4;
            collision.gameObject.transform.rotation = Quaternion.Euler(temp);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyReference = collision.gameObject;
            Vector3 temp = collision.gameObject.transform.rotation.eulerAngles;
            temp.z = 0;
            collision.gameObject.transform.rotation = Quaternion.Euler(temp);
        }
    }
}
