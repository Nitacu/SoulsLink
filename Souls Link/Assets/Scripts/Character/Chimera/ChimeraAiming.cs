using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAiming : PlayerAiming
{
    ChimeraController _controller;

    [SerializeField] private GameObject _chimeraCrosshair;
    [SerializeField] private float _chimeraCrosshairDistance = 2;

    private void Awake()
    {
        _controller = GetComponent<ChimeraController>();
    }

    private void Start()
    {
        _aimDirection = Vector2.right;
    }

    protected override void Update()
    {
        setAim();
        setCrossHair();
    }

    private void setAim()
    {
        Vector2 movement = _controller.Movement;
        if (movement.magnitude > 0)
        {
            _aimDirection = _controller.Movement;
            _aimDirection.Normalize();
        }
    }

    private void setCrossHair()
    {
        if (_chimeraCrosshair != null)
        {
            Vector2 directionAim = _aimDirection;
            directionAim.Normalize();

            _chimeraCrosshair.transform.localPosition = directionAim * _chimeraCrosshairDistance;
        }
    }
}
