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
            Debug.Log(_player.position);
            if (!Physics2D.Raycast(_thisCharacter.Value.position, (_player.position - _thisCharacter.Value.position), 5, _layerWalls))
            {
                Debug.Log("entro");
                target.Value = _player.transform;
                return true;
            }
        }

        return false;
    }

    public Transform playerInRange()
    {
        Collider2D[] ray = Physics2D.OverlapCircleAll(_thisCharacter.Value.position, _radio, _layerPlayer);
        
        if (target.Value != null)
        {     
            foreach (Collider2D player in ray)
            {
                if (player.gameObject == _player.gameObject)
                {
                    return player.transform;
                }
            }
        }

        if (ray.Length == 0)
            return null;
        else
            return ray[0].transform;
    }
}
