using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometDashControl : MonoBehaviour
{
    public GameObject _playerReference;
    public float stunDuration = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().GetStunned(stunDuration);
            _playerReference.GetComponent<CometDash>().resetDash();
        }
    }

    public void setStun(float _stunDurationn, GameObject _reference)
    {
        stunDuration = _stunDurationn;
        _playerReference = _reference;
    }


}
