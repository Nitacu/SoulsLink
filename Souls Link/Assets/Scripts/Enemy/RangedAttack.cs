using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RangedAttack : Action
{
    public SharedTransform target;
    public SharedTransform _thisCharacter;

    public override TaskStatus OnUpdate()
    {
        if (target.Value != null)
        {
            GetComponent<SimpleEnemyController>().attack(target.Value.gameObject);
        }

        return TaskStatus.Success;
    }
}
