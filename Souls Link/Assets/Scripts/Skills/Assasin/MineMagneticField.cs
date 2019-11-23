using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMagneticField : MonoBehaviour
{
    private GameObject enemy;
    private bool moveToEnemy = false;

    // Update is called once per frame
    void Update()
    {
        if (moveToEnemy)
        {
            //moveTowardsEnemy
            float step = 6f * Time.deltaTime;
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, enemy.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.gameObject;
            moveToEnemy = true;
        }
    }
}
