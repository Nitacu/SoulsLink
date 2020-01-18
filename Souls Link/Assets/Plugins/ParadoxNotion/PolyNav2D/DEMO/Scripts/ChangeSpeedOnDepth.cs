using PolyNav;
using UnityEngine;

//This script will change the agent position based on it's position.y value from a min to a max.
//This works as it is, but it's mostly to see how you could do it.
public class ChangeSpeedOnDepth : MonoBehaviour
{

    //when y = 5, speed = 1
    public float maxY = 5;
    public float maxYSpeed = 1;

    //when y = 0, speed = 5
    public float minY = 0;
    public float minYSpeed = 5;

    public AnimationCurve interpolation = AnimationCurve.Linear(0, 0, 1, 1);

    private PolyNavAgent _agent;
    private PolyNavAgent agent {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    void Update() {
        var normalizedY = Mathf.InverseLerp(minY, maxY, transform.position.y);
        agent.maxSpeed = Mathf.Lerp(minYSpeed, maxYSpeed, interpolation.Evaluate(normalizedY));
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawLine(new Vector3(-100, maxY, 0), new Vector3(100, maxY, 0));
        Gizmos.DrawLine(new Vector3(-100, minY, 0), new Vector3(100, minY, 0));
    }
}
