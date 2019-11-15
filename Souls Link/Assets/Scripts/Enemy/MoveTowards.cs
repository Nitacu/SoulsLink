using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowards : Action
{
    // The speed of the object
    public float speed = 0;
    // The transform that the object is moving towards
    public SharedTransform target;

    private Vector2 direction;

    public override void OnStart()
    {
        GetComponent<PolyNavAgent>().enabled = false;
    }

    public override TaskStatus OnUpdate()
    {
        // Return a task status of success once we've reached the target
        if (Vector2.SqrMagnitude(transform.position - target.Value.position) < 2f)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            return TaskStatus.Success;
        }
        else
        {
            // We haven't reached the target yet so keep moving towards it
            direction = target.Value.position - transform.position;
            direction.Normalize();
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }

        return TaskStatus.Running;
    }

}
