using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundByAim : MonoBehaviour
{
    private Vector2 _aimVector;

    private void Update()
    {
        rotateAround();
    }

    private void rotateAround()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _aimVector = mousePosition - (Vector2)transform.position;
        _aimVector.Normalize();

        float rot = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0,0,rot);
    }
}
