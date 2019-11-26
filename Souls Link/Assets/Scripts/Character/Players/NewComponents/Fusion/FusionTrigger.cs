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

    private bool _checkingUnFusion = false;

    private ChimeraController _currentChimeraParent;
    public ChimeraController CurrentChimeraParent
    {
        get { return _currentChimeraParent; }
        set { _currentChimeraParent = value; }
    }

    [SerializeField] float _radiusFusionCheck;
    [SerializeField] List<GameObject> _componentsToDeactivate;

    private void Awake()
    {
        _inputControl = new PlayerInputActions();
    }


    private bool _isOnFusion;
    public bool IsOnFusion
    {
        get { return _isOnFusion; }
        set { _isOnFusion = value; }
    }

    private int _onFusionID;
    public int OnFusionID
    {
        get { return _onFusionID; }
        set { _onFusionID = value; }
    }

    private void Update()
    {

        if (_checkingFusion)
        {
            findPlayerToFusion();
        }
        else if (_checkingUnFusion)
        {
            sendUnFusion();
        }
    }

    private void sendUnFusion()
    {
        if (CurrentChimeraParent != null)
        {
            CurrentChimeraParent.sendUnFusion(true, OnFusionID);
        }
    }

    private void findPlayerToFusion()
    {
        //Circle Raycast para encontrar jugadores
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(transform.position, _radiusFusionCheck);

        GameObject hostFindedGO = null;
        GameObject chimeraFindedGO = null;

        if (_colliders.Length > 0)
        {
            //Evaluar cada jugador
            foreach (var hit in _colliders)
            {
                GameObject other = hit.gameObject;

                if (other.GetComponent<FusionHost>())
                {
                    hostFindedGO = other;
                }
                if (other.GetComponent<ChimeraController>())
                {
                    chimeraFindedGO = other;
                }
            }
        }

        if (hostFindedGO != null)
        {
            //Encontré un host existente

            if (!hostFindedGO.GetComponent<FusionHost>().playerIsAttached(gameObject))//Saber si soy parte del host
            {
                //Si no soy parte del host puedo fusionar
                hostFindedGO.GetComponent<FusionHost>().addPlayerToFusion(gameObject);
            }
        }
        else
        {
            //No encontré host pero si una chimera
            if (chimeraFindedGO != null)
            {
                //Unirme a la chimera
                chimeraFindedGO.GetComponent<ChimeraController>().addNewPlayer(gameObject);
                DeactivateComponentsOnFusion();
            }
            else
            {

                //No encontré ningún host


                //Crear un nuevo host
                _myFusionHosting = Instantiate(_hostPrefab, gameObject.transform, false);
                //Añadirme al host
                _myFusionHosting.GetComponent<FusionHost>().addPlayerToFusion(gameObject);
            }
        }
    }


    public void DeactivateComponentsOnFusion()
    {
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(false);
        }

        //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void ActiveComponentsOnFusion()
    {
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(true);
        }

        //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        GetComponent<PlayerMovement>().RigidBodyPlayer = rb;

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnFusion(InputValue action)
    {
        float _actionPressed = action.Get<float>();
        if (_actionPressed == 1)//Pressed
        {
            if (IsOnFusion && !ChekingFusion)
            {
                _checkingUnFusion = true;
            }
            else
            {
                _checkingFusion = true;

            }

        }
        else if (_actionPressed == 0)//Released
        {
            _checkingFusion = false;
            _checkingUnFusion = false;
            if (_myFusionHosting != null) Destroy(_myFusionHosting);
            if (IsOnFusion) CurrentChimeraParent.sendUnFusion(false, OnFusionID);

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
