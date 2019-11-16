using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffectControl : MonoBehaviour
{

    private GameObject enemy;
    private float autoDestructionTime;
    private bool _enemyWasWalking;

    public void setEffect(GameObject _enemy, float _effectDuration, bool enemyWalking)
    {
        _enemyWasWalking = enemyWalking;
        enemy = _enemy;
        autoDestructionTime = _effectDuration;
        Invoke("destructMyself", autoDestructionTime);
    }

    private void destructMyself()
    {
        if (_enemyWasWalking)
        {
            enemy.GetComponent<SimpleEnemyController>().canWalk = true;
        }
        
        Destroy(gameObject);
    }
}
