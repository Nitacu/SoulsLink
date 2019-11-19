using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBallsControl : MonoBehaviour
{

    public float _ballDamage = 20;
    private float contBallNumber = 0;
    private float numberOfBalls = 4;

    public void doDamage(GameObject enemy)
    {
        enemy.GetComponent<SimpleEnemyController>().recieveDamage(_ballDamage);
        contBallNumber++;
        if(contBallNumber >= numberOfBalls)
        {
            Destroy(gameObject);
        }
        
    }
    
}
