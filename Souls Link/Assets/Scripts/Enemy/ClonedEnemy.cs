using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ClonedEnemy : Action
{

    public override TaskStatus OnUpdate()
    {

        if (GetComponent<EnemyMeleeMultiplayerController>().isMine())
        {   
            return TaskStatus.Success;
        }
        else
        {
            GetComponent<BehaviorTree>().enabled = false;
            GetComponent<PolyNavAgent>().enabled = false;
        }

        return TaskStatus.Running;
    }
}
