using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    private float _tornadoLifeTime = 0;
    private float _damage = 0;
    private GameObject _tornado;
    public GameObject magneticField;
    private float offsetY = -0.3f;
    private float electricCost = 45;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            applyDamage(collision.gameObject);
            /*
            Vector3 newTornadoPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking(false);
            GameObject temp = Instantiate(_tornado, newTornadoPosition, Quaternion.identity);
            temp.GetComponent<TornadoController>().destroyTornado(_tornadoLifeTime);           
            Destroy(gameObject);
            */
        }
    }

    public void applyDamage(GameObject collision)
    {
        Vector3 newTornadoPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking(false);
        GameObject temp = Instantiate(_tornado, newTornadoPosition, Quaternion.identity);
        temp.GetComponent<TornadoController>().destroyTornado(_tornadoLifeTime);
        Destroy(gameObject);
    }

    public void setBomb(float tornadoLifeTime, float damage, GameObject tornado, float chargePercent, GameObject playerReference)
    {
        
        if (chargePercent >= electricCost)
        {
            magneticField.SetActive(true);
            playerReference.GetComponent<Dash>().consumeChargeBar(electricCost);
        }
        _tornadoLifeTime = tornadoLifeTime;
        _damage = damage;
        _tornado = tornado;
    }
}
