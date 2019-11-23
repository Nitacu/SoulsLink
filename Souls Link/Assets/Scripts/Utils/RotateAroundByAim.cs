using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundByAim : MonoBehaviour
{
    private Vector2 _aimVector;

    private void Update()
    {
        
    }

    public void rotateAround(Vector2 direction)
    {
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0,0,rot);
    }
}
