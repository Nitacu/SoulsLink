using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmashController : MonoBehaviour
{
    private float _force;
    private float _damage;
    private float _duration;
    Vector2 _direction;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(PhotonNetwork.IsMasterClient)
            collision.GetComponent<SimpleEnemyController>().getKnocked(_force,_damage, _duration, _direction);
        }
    }

    public void setSmash(Vector2 direction, float force, float damage, float duration)
    {
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, rot);
        _force = force;
        _damage = damage;
        _duration = duration;
        _direction = direction;
    }
}
