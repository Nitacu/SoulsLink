using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MeleeAttack : Action
{
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (Physics2D.OverlapCircle(target.Value.position,0.1f).CompareTag("Player"))
        {
            Debug.Log("Pau le pegue");
        }

        return TaskStatus.Success;
    }

}
