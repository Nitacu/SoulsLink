using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private float _distanceEffect = 1;

    private AimCursor _aimCursor;

    private void Start()
    {
        _aimCursor = GetComponent<AimCursor>();
    }

    private void Update()
    {
        Vector2 _origin = transform.position;
        Vector2 _direction = _aimCursor.AimVector;

        RaycastHit2D hit = Physics2D.Raycast(_origin, _direction, _distanceEffect);
        Debug.DrawRay(_origin, _direction * _distanceEffect, Color.red);
    }

}
