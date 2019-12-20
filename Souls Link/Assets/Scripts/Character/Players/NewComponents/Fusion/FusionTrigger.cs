using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class FusionTrigger : MonoBehaviour
{
    PlayerInputActions _inputControl;

    public GameManager.Characters _characterType;

    private bool _checkingFusion = false;
    public bool ChekingFusion
    {
        get { return _checkingFusion; }
    }

    [SerializeField] private ChimeraController _currentChimeraParent;
    public ChimeraController CurrentChimeraParent
    {
        get { return _currentChimeraParent; }
        set { _currentChimeraParent = value; }
    }

    [SerializeField] List<GameObject> _componentsToDeactivate;

    [SerializeField] List<Component> _skills = new List<Component>();

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    public DelegateMultiplayerController _isHost;
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

    [SerializeField] private int _onFusionID;
    public int OnFusionID
    {
        get { return _onFusionID; }
        set { _onFusionID = value; }
    }

    private void sendUnFusion(bool forced)
    {
        if (CurrentChimeraParent != null)
        {
            CurrentChimeraParent.sendUnFusion(true, OnFusionID, forced);
        }
    }

    public void DeactivateComponentsOnFusion()
    {
        //Desactivar Skills
        PlayerSkills playerSkills = GetComponent<PlayerSkills>();
        playerSkills.enabled = false;

        Skill[] skills = GetComponents<Skill>();
        foreach (var item in skills)
        {
            item.enabled = false;
        }

        //Desactivar componentes
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(false);
        }

        Destroy(gameObject.GetComponent<Rigidbody2D>());
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<PlayerHPControl>().enabled = false;

    }

    public void ActiveComponentsOnFusion()
    {
        //Activar Skills
        PlayerSkills playerSkills = GetComponent<PlayerSkills>();
        playerSkills.enabled = true;

        Skill[] skills = GetComponents<Skill>();
        foreach (var item in skills)
        {
            item.enabled = true;
        }

        //Activar componentes
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(true);
        }

        Rigidbody2D rb;
        //añadirm rigidbody
        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();          
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody2D>();            
        }
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        GetComponent<PlayerMovement>().RigidBodyPlayer = rb;

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<PlayerHPControl>().enabled = true;
    }

    private void OnFusion(InputValue action)
    {
        if (_isMine())
        {
            float _actionPressed = action.Get<float>();
            if (_actionPressed == 1)//Pressed
            {
                AddMeToGeneralHost();//me agrega al host de la quimera
                _pushAddMeToGeneralHost(); //se agrega en otras maquinas
            }
            else if (_actionPressed == 0)//Released
            {
                GetoutToGeneralHost(); //lo saca del host de la quimera
                _pushGetoutToGeneralHost(); // lo saca en las demas maquinas
            }
        }
    }

    private void OnUnFusion(InputValue action)
    {

        if (_isMine())
        {
            float _actionPressed = action.Get<float>();
            if (_actionPressed == 1)//Pressed
            {
                //para separarse
                if (IsOnFusion)
                {
                    sendUnFusionToChimera(true, false);
                }

            }
            else if (_actionPressed == 0)//Released
            {
                if (IsOnFusion)
                {
                    sendUnFusionToChimera(false, false);
                }
            }
        }
    }

    public void sendUnFusionToChimera(bool state, bool forced)
    {
        if (CurrentChimeraParent != null)
        {
            Debug.Log("NOMBRE DE LA QUIMERA " + CurrentChimeraParent.gameObject.name);
            if (_isHost())
                CurrentChimeraParent.sendUnFusion(state, OnFusionID, forced);
            else
                CurrentChimeraParent._sendUnFusion(state, OnFusionID, forced);
        }
    }

    bool alreadyInHost = false;
    public void AddMeToGeneralHost()
    {
        FusionManager fusionManager = FindObjectOfType<FusionManager>();

        if (fusionManager != null)
        {
            fusionManager.addMeToFusion(gameObject);
            alreadyInHost = true;
        }
    }

    public void GetoutToGeneralHost()
    {
        FusionManager fusionManager = FindObjectOfType<FusionManager>();

        if (fusionManager != null)
        {
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
        Debug.Log("Set On Fusion entre (" + chimeraController.name + " y " + gameObject.name + ")");

        //Saber si estoy en fusion y mi id para la chimera
        _isOnFusion = true;
        _onFusionID = fusionID;

        //Informaciónd e la chimera en la que estoy
        _currentChimeraParent = chimeraController.GetComponent<ChimeraController>();

        transform.SetParent(chimeraController.transform);
        transform.localPosition = Vector3.zero;

        //assingSkillsTochimera(chimeraController);

        Debug.Log("Set On Fusion SALÍ");
    }

    public void setOnUnFusion()
    {
        Debug.Log("Set On UN Fusion" + CurrentChimeraParent.name + " y " + gameObject.name + ")");

        IsOnFusion = false;
        CurrentChimeraParent = null;
        ActiveComponentsOnFusion();

        transform.parent = null;

        Debug.Log("Set On UNFusion SALÍ");
    }

    public bool availableToFusion()
    {
        if (!IsOnFusion)//si yo no estoy en fusion siempre me puedo fusionar
        {
            return true;
        }
        else//sino mirar si todos mis compañeros en al chimera quieren fusionarse
        {
            if (_currentChimeraParent != null)
            {
                //saber si todos los hijos de mi chimera padre estamos en el host para fusionarnos
                bool allInHostToFusion = true;
                FusionManager fusionManager = FindObjectOfType<FusionManager>();

                foreach (var item in _currentChimeraParent.Players)
                {
                    if (!fusionManager._playersToFusion.ToList().Contains(item))
                    {
                        allInHostToFusion = false;
                    }
                }

                return allInHostToFusion;
            }
            else
            {
                return true;
            }
        }
    }
}

