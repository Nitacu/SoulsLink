using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PatrolMovement : Action
{

    public List<Vector2> WPoints = new List<Vector2>();
    public float delayBetweenPoints = 0;
    int index = 0;
    private PolyNav.PolyNavAgent _agent;
    private PolyNav.PolyNavAgent agent
    {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNav.PolyNavAgent>(); }
    }

    public override void OnStart()
    {
        agent.OnDestinationReached += MoveRandom;
        agent.OnDestinationInvalid += MoveRandom;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        if (WPoints.Count > 0)
        {
            MoveRandom();
        }
    }

    bool canWaitAndMove = true;

    void MoveRandom()
    {
        if (canWaitAndMove)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    Vector2 lasPositionDestination = new Vector2();
    Vector2 newPositionDestination = new Vector2();

    IEnumerator WaitAndMove()
    {
        canWaitAndMove = false;

        GetComponent<SimpleEnemyController>().Anim.SetFloat("Velocity", 0);
        yield return new WaitForSeconds(0);
        newPositionDestination = WPoints[index];

        if (newPositionDestination != lasPositionDestination)
        {
            lasPositionDestination = newPositionDestination;
            agent.SetDestination(newPositionDestination);
            index++;
            if (WPoints.Count == index)
            {
                index = 0;
            }
        }

        StartCoroutine(allowStartWaitAndMove());
    }

    IEnumerator allowStartWaitAndMove()
    {
        yield return new WaitForEndOfFrame();
        canWaitAndMove = true;
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < WPoints.Count; i++)
        {
            Gizmos.DrawSphere(WPoints[i], 0.05f);
        }
    }


}
