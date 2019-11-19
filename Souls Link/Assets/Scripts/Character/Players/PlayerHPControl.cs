using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPControl : MonoBehaviour
{
    [SerializeField] private float _playerHealth;

    #region PLAYER STATES
     private bool canRecieveDamage = true;
     private bool deflectsDamage = false;
    #endregion

    public void recieveDamage(float damage, GameObject attackingEnemy)
    {
        if (canRecieveDamage)
        {
            _playerHealth -= damage;
            if (_playerHealth < 0)
            {
                Destroy(gameObject);
            }
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("backToNormal", 0.5f);
        }
        else
        {
            if (deflectsDamage)
            {
                attackingEnemy.GetComponent<SimpleEnemyController>().recieveDamage(damage);
            }
        }
    }

    public void setReflectiveMode()
    {
        canRecieveDamage = false;
        deflectsDamage = true;
    }

    public void setNormalMode()
    {
        canRecieveDamage = true;
        deflectsDamage = false;
    }

    private void backToNormal()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
