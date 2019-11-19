using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MeleeAttack : Action
{
    public SharedTransform target;
    public SharedTransform _thisCharacter;

    public override TaskStatus OnUpdate()
    {
        if (target.Value != null)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(_thisCharacter.Value.position, 0.75f);

            foreach (Collider2D obj in col)
            {
                if (obj.GetComponentInChildren<Animator>().gameObject == target.Value.gameObject)
                {
                    GetComponent<SimpleEnemyController>().attack(target.Value.gameObject);
                }
            }
        }

        return TaskStatus.Success;
    }

}
