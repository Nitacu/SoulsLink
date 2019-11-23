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
            Vector3 newTornadoPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking(false);
            GameObject temp = Instantiate(_tornado, newTornadoPosition, Quaternion.identity);
            temp.GetComponent<TornadoController>().destroyTornado(_tornadoLifeTime);
            Destroy(gameObject);
        }
    }

    public void setBomb(float tornadoLifeTime, float damage, GameObject tornado, float chargePercent)
    {
        if(chargePercent >= 50)
        {
            magneticField.SetActive(true);
        }
        _tornadoLifeTime = tornadoLifeTime;
        _damage = damage;
        _tornado = tornado;
    }
}
