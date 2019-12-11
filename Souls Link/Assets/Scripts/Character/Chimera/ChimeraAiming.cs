using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAiming : MonoBehaviour
{
    ChimeraController _controller;

    private Vector2 _aimDirection;

    private void Awake()
    {
        _controller = GetComponent<ChimeraController>();
    }

    private void Update()
    {
        _aimDirection = _controller.Movement;
    }

}
