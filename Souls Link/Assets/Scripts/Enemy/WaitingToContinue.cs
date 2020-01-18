using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WaitingToContinue : Action
{
    public SharedFloat waitTime = 0.1f;
    private float startTime;
    private float waitDuration;

    public override void OnStart()
    {
        // Remember the start time.
        startTime = Time.time;

        waitDuration = waitTime.Value;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public override TaskStatus OnUpdate()
    {
        // The task is done waiting if the time waitDuration has elapsed since the task was started.
        if (startTime + waitDuration < Time.time)
        {
            GetComponent<PolyNav.PolyNavAgent>().enabled = true;
            return TaskStatus.Success;
        }
        else
        {
            GetComponent<SimpleEnemyController>().Anim.SetFloat("Velocity",0);
            GetComponent<SimpleEnemyController>().Anim.SetBool("Can_Walk", false);
            GetComponent<PolyNav.PolyNavAgent>().enabled = false;
        }
        // Otherwise we are still waiting.
        return TaskStatus.Running;
    }

}
