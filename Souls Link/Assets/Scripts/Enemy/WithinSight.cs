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
        Collider2D ray = Physics2D.OverlapCircle(transform.position, _radio, _layerPlayer);

        if (ray)
        {

            if (!Physics2D.Raycast(transform.position, (transform.position - ray.transform.position), 5, _layerWalls))
            {
                target.Value = ray.transform;
                return true;
            }
        }

        return false;
    }
}
