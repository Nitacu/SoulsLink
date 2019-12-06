using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WithinSight : Conditional
{
    public float _radio;

    public LayerMask _layerPlayer;
    public LayerMask _layerWalls;

    // The tag of the targets
    public string targetTag;
    // Set the target variable when a target has been found so the subsequent tasks know which object is the target
    public SharedTransform target;
    public SharedTransform _thisCharacter;
    private Transform _player;
    // A cache of all of the possible targets

    public override TaskStatus OnUpdate()
    {

        if (withinSight())
        {
            return TaskStatus.Success;
        }

        target.Value = null;
        return TaskStatus.Failure;
    }
    // Returns true if targetTransform is within sight of currenttransform
    public bool withinSight()
    {
        
         _player = playerInRange();


         if (_player != null)
         {
            if (!Physics2D.Linecast(_thisCharacter.Value.position, _player.position , _layerWalls))
            {
                target.Value = _player;
                return true;
            }
         }
         return false;
    }

    public Transform playerInRange()
    {
        Collider2D[] ray = Physics2D.OverlapCircleAll(_thisCharacter.Value.position, _radio, _layerPlayer);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        //if (target.Value != null)
        //{     
            foreach (Collider2D player in ray)
            {                
                    float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestPlayer = player.gameObject;
                    }
                    //return player.transform;               
            }

            //set target to player
            if (nearestPlayer != null && shortestDistance <= _radio)
            {
                return nearestPlayer.transform;
            }
            else
            {
                return null;
            }
        //}

       /* if (ray.Length == 0)
            return null;
        else
            return ray[0].transform;*/
    }

    public Transform ConeRaycast()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(_thisCharacter.Value.position, _radio, _layerPlayer);
        Vector3 characterToCollider;
        float dot;

        if (target.Value != null)
        {
            foreach (Collider2D player in cols)
            {
                if (player.gameObject == _player.gameObject)
                {
                    characterToCollider = (player.transform.position - _thisCharacter.Value.position).normalized;
                    dot = Vector3.Dot(characterToCollider, _thisCharacter.Value.forward);
                    if (dot >= Mathf.Cos(55))
                    {
                        //objectSpotted = true;
                        return player.transform;
                    }
                    else
                    {
                        //objectSpotted = false;
                    }
                }
            }
        }

        if (cols.Length == 0)
            return null;
        else
            return cols[0].transform;
    }
}
