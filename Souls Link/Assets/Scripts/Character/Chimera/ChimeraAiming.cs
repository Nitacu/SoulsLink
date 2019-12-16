using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAiming : PlayerAiming
{
    ChimeraController _controller;

    [SerializeField] private GameObject _chimeraCrosshair;
    [SerializeField] private float _chimeraCrosshairDistance = 2;

    public ChimeraController Controller { get => _controller; set => _controller = value; }

    private void Awake()
    {
        Controller = GetComponent<ChimeraController>();
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

    public void setAim()
    {
        Vector2 movement = Controller.Movement;
        if (movement.magnitude > 0)
        {
            _aimDirection = Controller.Movement;
            _aimDirection.Normalize();
        }
    }

    public void setAim(Vector2 aim)
    {
        Vector2 movement = aim;
        if (movement.magnitude > 0)
        {
            _aimDirection = Controller.Movement;
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
