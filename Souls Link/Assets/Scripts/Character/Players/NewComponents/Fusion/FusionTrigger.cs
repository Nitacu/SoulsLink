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

    [SerializeField] List<Component> _skills = new List<Component>();

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    public delegate void DelegateMultiplayerControllerVoid();
    public DelegateMultiplayerControllerVoid _pushAddMeToGeneralHost;
    public DelegateMultiplayerControllerVoid _pushGetoutToGeneralHost;
    public delegate string DelegateMultiplayerControllerID();
    public DelegateMultiplayerControllerID _myID;
    #endregion

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

    private void sendUnFusion()
    {
        if (CurrentChimeraParent != null)
        {
            CurrentChimeraParent.sendUnFusion(true, OnFusionID);
        }
    }

    public void DeactivateComponentsOnFusion()
    {
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(false);
        }

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
        if (_isMine())
        {
            float _actionPressed = action.Get<float>();
            if (_actionPressed == 1)//Pressed
            {
                if (!IsOnFusion)
                {
                    AddMeToGeneralHost();//me agrega al host de la quimera
                    _pushAddMeToGeneralHost(); // se agrega en otras maquinas
                }
                else
                {
                    //para separarse
                    CurrentChimeraParent.sendUnFusion(true, OnFusionID);
                }
            }
            else if (_actionPressed == 0)//Released
            {
                if (alreadyInHost && !IsOnFusion)
                {
                    GetoutToGeneralHost(); //lo saca del host de la quimera
                    _pushGetoutToGeneralHost(); // lo saca en las demas maquinas
                }
                if (IsOnFusion) CurrentChimeraParent.sendUnFusion(false, OnFusionID);

            }
        }
    }

    bool alreadyInHost = false;
    public void AddMeToGeneralHost()
    {
        FusionManager fusionManager = FindObjectOfType<FusionManager>();

        if (fusionManager != null)
        {
            Debug.Log("Añadirme al host");
            fusionManager.addMeToFusion(gameObject);
            alreadyInHost = true;
        }
    }

    public void GetoutToGeneralHost()
    {
        FusionManager fusionManager = FindObjectOfType<FusionManager>();

        if (fusionManager != null)
        {
            Debug.Log("Sacarme del host");
            fusionManager.getOutToFusion(gameObject);
            alreadyInHost = false;
        }
    }

    public void assingSkillsTochimera(GameObject chimera)
    {
        Skill[] skills = GetComponents<Skill>();

        foreach (var skill in skills)
        {
            System.Type type = skill.GetType();
            Skill compo = chimera.GetComponent(type) as Skill;
            if (compo != null)
            {
                compo.enabled = true;
            }
        }
    }

    public void setOnFusion(GameObject chimeraController, int fusionID)
    {
        //Saber si estoy en fusion y mi id para la chimera
        _isOnFusion = true;
        _onFusionID = fusionID;

        //Informaciónd e la chimera en la que estoy
        _currentChimeraParent = chimeraController.GetComponent<ChimeraController>();
        transform.SetParent(chimeraController.transform);
        transform.localPosition = Vector3.zero;

        assingSkillsTochimera(chimeraController);
    }
}

