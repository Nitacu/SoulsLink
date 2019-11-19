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
    private PolyNavAgent _agent;
    private PolyNavAgent agent
    {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    public override void OnStart()
    {
        agent.OnDestinationReached += MoveRandom;
        agent.OnDestinationInvalid += MoveRandom;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        GetComponent<PolyNavAgent>().enabled = true;

        StopCoroutine(WaitAndMove());

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
        GetComponent<SimpleEnemyController>().Anim.SetFloat("Velocity",0);
        yield return new WaitForSeconds(0);        
        Vector2 endPosition = WPoints[index];
        agent.SetDestination(endPosition);
        index++;
        if (WPoints.Count == index)
        {
            index = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < WPoints.Count; i++)
        {
            Gizmos.DrawSphere(WPoints[i], 0.05f);
        }
    }
    
    
}
