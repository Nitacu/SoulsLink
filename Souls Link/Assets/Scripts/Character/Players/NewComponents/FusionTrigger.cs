using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FusionTrigger : MonoBehaviour
{
    PlayerInputActions _inputControl;

    [SerializeField] private bool _checkingFusion = false;
    public bool ChekingFusion
    {
        get { return _checkingFusion; }
    }

    [SerializeField] float _radiusFusionCheck;
    [SerializeField] LayerMask _playersFusionMask;

    private void Awake()
    {
        _inputControl = new PlayerInputActions();

    }


    private void Update()
    {
        if (_checkingFusion)
        {
            findPlayerToFusion();
        }
    }

    private void findPlayerToFusion()
    {
        //Circle Raycast para encontrar jugadores
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(transform.position, _radiusFusionCheck, _playersFusionMask);

        if (_colliders.Length > 0)
        {
            //Evaluar cada jugador
            foreach (var hit in _colliders)
            {
                GameObject other = hit.gameObject;
                if (other.GetComponent<FusionTrigger>() && other != gameObject)
                {
                    FusionTrigger otherFusionTrigger = other.GetComponent<FusionTrigger>();
                    if (otherFusionTrigger._checkingFusion)
                    {
                        Fusion();
                        otherFusionTrigger.Fusion();
                    }
                }
            }
        }
    }

    public void Fusion()
    {
        Debug.Log("Fusionar");
    }

    private void OnFusion(InputValue action)
    {
        float _actionPressed = action.Get<float>();
        if (_actionPressed == 1)//Pressed
        {
            _checkingFusion = true;
        }
        else if (_actionPressed == 0)//Released
        {
            _checkingFusion = false;
        }
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = Color.yellow;
        gizmoColor.a = 0.3f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, _radiusFusionCheck);
    }


}
