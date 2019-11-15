using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PatrolMovement : Action
{
    public List<Vector2> WPoints = new List<Vector2>();
    public float delayBetweenPoints = 0f;
    private int index = 0;
    private PolyNavAgent _agent;
    private PolyNavAgent agent
    {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    public override void OnStart()
    {
        agent.OnDestinationReached += MoveRandom;
        agent.OnDestinationInvalid += MoveRandom;

        GetComponent<PolyNavAgent>().enabled = true;

        if (WPoints.Count > 0)
        {
            MoveRandom();
        }
    }

    void MoveRandom()
    {       
        StartCoroutine(WaitAndMove());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(delayBetweenPoints);
        Vector2 endPosition = WPoints[Random.Range(0, WPoints.Count)];
        agent.SetDestination(endPosition);
        GetComponent<Animator>().SetFloat("Velocity", 1);
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < WPoints.Count; i++)
        {
            Gizmos.DrawSphere(WPoints[i], 0.05f);
        }
    }


}
