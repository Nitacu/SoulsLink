using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FusionTrigger : MonoBehaviour
{
    PlayerInputActions _inputControl;

    [SerializeField] private GameObject _hostPrefab;
    private GameObject _myFusionHosting;

    private bool hostingFusion = false;

    private bool _checkingFusion = false;
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
        /*
        if (_checkingFusion)
        {
            findPlayerToFusion();
        }
        */
    }

    private void findPlayerToFusion()
    {
        //Circle Raycast para encontrar jugadores
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(transform.position, _radiusFusionCheck);

        GameObject hostFindedGO = null;
        bool _hostFinded = false;

        if (_colliders.Length > 0)
        {
            //Evaluar cada jugador
            foreach (var hit in _colliders)
            {
                GameObject other = hit.gameObject;
                Debug.Log("Hit with: " + other.name);

                if (other.GetComponent<FusionHost>())
                {
                    _hostFinded = true;
                    hostFindedGO = other;
                    Debug.Log("HostFinded");
                }

                /*
                if (other.GetComponent<FusionTrigger>() && other != gameObject)
                {
                    FusionTrigger otherFusionTrigger = other.GetComponent<FusionTrigger>();

                    if (other.GetComponent<FusionHost>())
                    {
                        _hostFinded = true;
                        Debug.Log("HostFinded");
                    }
                    if (otherFusionTrigger._checkingFusion)
                    {
                        
                    }
                }*/
            }

            if (!_hostFinded && !hostingFusion)
            {
                _myFusionHosting = Instantiate(_hostPrefab, gameObject.transform, false);
                _myFusionHosting.GetComponent<FusionHost>().addPlayerToFusion(gameObject);
                hostingFusion = true;
            }
            else
            {
                hostFindedGO.GetComponent<FusionHost>().addPlayerToFusion(gameObject);
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
            findPlayerToFusion();
        }
        else if (_actionPressed == 0)//Released
        {
            _checkingFusion = false;
            //if (_myFusionHosting != null) Destroy(_myFusionHosting);
            //hostingFusion = false;
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
