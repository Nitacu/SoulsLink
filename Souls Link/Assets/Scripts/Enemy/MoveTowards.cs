using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowards : Action
{
    // The speed of the object
    public float speed = 0;
    // The transform that the object is moving towards
    public SharedTransform target;
    public SharedTransform _thisCharacter;
    private Transform _destiny;
    private Vector2 direction;

    public override void OnStart()
    {
        GetComponent<PolyNavAgent>().enabled = false;
    }

    public override TaskStatus OnUpdate()
    {
        // Return a task status of success once we've reached the target
        _destiny = target.Value;

        if (inRange())
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            return TaskStatus.Success;
        }
        else
        {
            // We haven't reached the target yet so keep moving towards it
            GetComponent<PolyNavAgent>().enabled = false;
            direction = target.Value.position - transform.position;
            direction.Normalize();
            if(GetComponent<SimpleEnemyController>().isStunned == true)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = direction * speed;
            }
            
            GetComponent<SimpleEnemyController>().changeOrientation(direction.x);
        }

        return TaskStatus.Running;
    }

    public bool inRange()
    {
        if (target.Value != null)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(_thisCharacter.Value.position, 0.75f);
            foreach (Collider2D obj in col)
            {
                if (obj.gameObject == _destiny.gameObject)
                {
                    Debug.Log("en rango");
                    return true;
                }
            }
        }

        return false;
    }

}
