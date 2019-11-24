using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpController : SimpleEnemyController
{
    public GameObject player;
    [SerializeField] private GameObject _projectile;
    private float angle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack(player);
        }
    }

    public override void attack(GameObject player)
    {
        //GameObject aux = Instantiate(_projectile,transform.position,Quaternion.identity);

        angle = Vector2.Angle(transform.position,player.transform.position);
        Debug.Log(angle *Mathf.Rad2Deg);

        //aux.transform.localEulerAngles = new Vector3(0,0,angle);

    }
}
