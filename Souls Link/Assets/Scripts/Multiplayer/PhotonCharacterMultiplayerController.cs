using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PhotonCharacterMultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerMovement _playerMovement;
    private PlayerSkills _playerSkills;
    private PlayerAiming _playerAiming;
    private PlayerHPControl _hPControl;
    private FusionTrigger _fusionTrigger;
    private FusionManager _fusionManager;

    [SerializeField] private GameObject _playerHUD;
    [SerializeField] private PhotonView _photonView;

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
    private const string ADD_ME_HOST = "addMeToGeneralHost";
    private const string GET_OUT_HOST = "getoutToGeneralHost";
    private const string PLAYER_SKILLS = "playerOnSkill";
    private const string PLAYER_AIMING = "playerAiming";
    private const string HEALTH = "Health";
    private const string FLIP = "Flip";
    #endregion

    private void Start()
    {
        if (!isMine())
        {
            Destroy(GetComponent<PlayerInput>());
            Destroy(_playerHUD);
        }

        _hPControl = GetComponent<PlayerHPControl>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSkills = GetComponent<PlayerSkills>();
        _playerAiming = GetComponent<PlayerAiming>();
        _fusionTrigger = GetComponent<FusionTrigger>();
        _fusionManager = FindObjectOfType<FusionManager>();
        addDelegate();
        activeComponets();
    }

    public void addDelegate()
    {
        _playerMovement._isMine = new PlayerMovement.DelegateMultiplayerController(isMine);

        _playerSkills._isMine = new PlayerSkills.DelegateMultiplayerController(isMine);
        _playerSkills._pushValueSkill = new PlayerSkills.DelegateMultiplayerControllerSendSkill(pushValueSkill);

        _playerAiming._isMine = new PlayerAiming.DelegateMultiplayerController(isMine);

        _hPControl._isMine = new PlayerHPControl.DelegateMultiplayerController(isMine);
        _hPControl._destroySelf = new PlayerHPControl.DelegateMultiplayerControllerDestroy(destroySelf);
        _hPControl._changeHealth = new PlayerHPControl.DelegateMultiplayerControllerHealth(changeHealth);

        _fusionTrigger._isMine = new FusionTrigger.DelegateMultiplayerController(isMine);
        _fusionTrigger._pushAddMeToGeneralHost = new FusionTrigger.DelegateMultiplayerControllerVoid(pushAddMeToGeneralHost);
        _fusionTrigger._pushGetoutToGeneralHost = new FusionTrigger.DelegateMultiplayerControllerVoid(pushGetoutToGeneralHost);
        _fusionTrigger._myID = new FusionTrigger.DelegateMultiplayerControllerID(myID);

        _fusionManager._isHost = new FusionManager.DelegateMultiplayerController(isHost);
        _fusionManager._createdChimera = new FusionManager.DelegateMultiplayerControllerCreatedChimera(createdChimeta);
    }

    private void activeComponets()
    {
        _playerMovement.enabled = true;
        _playerSkills.enabled = true;
        _playerAiming.enabled = true;
        _hPControl.enabled = true;
        _fusionTrigger.enabled = true;
        _fusionManager.enabled = true;
    }

    #region funciones propias
    public string myID()
    {
        return _photonView.ViewID.ToString();
    }

    public bool isHost()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public void destroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    #endregion 

    #region apuntar
    public void pushVectorAiming(Vector3 vector)
    {
        _photonView.RPC(PLAYER_AIMING, RpcTarget.Others, vector);
    }

    [PunRPC]
    public void playerAiming(Vector3 vector)
    {
        _playerAiming.AimDirection = vector;

        if (vector.x < 0)
        {
            _playerMovement._flip = true;
        }
        else if (vector.x > 0)
        {
            _playerMovement._flip = false;
        }
    }
    #endregion

    #region skills
    //le envia a las otras maquinas los datos para que active las skills
    public void pushValueSkill(float value, float numberSkill)
    {
        _photonView.RPC(PLAYER_SKILLS, RpcTarget.Others, value, numberSkill, _playerAiming.AimDirection);
    }

    // carga los datos y activa la skill correspondiente
    [PunRPC]
    public void playerOnSkill(float value, float numberSkill, Vector2 vectorAiming)
    {

        Debug.Log("valor " + value + " numero de skill " + numberSkill + "vecto direccion" + vectorAiming);

        playerAiming(vectorAiming);

        if (value == 1)
        {
            switch (numberSkill)
            {
                case 1:
                    _playerSkills.Skill1PressDown1.Invoke();
                    break;

                case 2:
                    _playerSkills.Skill2PressDown1.Invoke();
                    break;

                case 3:
                    _playerSkills.Skill3PressDown1.Invoke();
                    break;

                case 4:
                    _playerSkills.Skill4PressDown1.Invoke();
                    break;
                case 5:
                    _playerSkills.DashPressDown.Invoke();
                    break;
            }
        }
        else if (value == 0)
        {
            switch (numberSkill)
            {
                case 1:
                    _playerSkills.Skill1PressUp1.Invoke();
                    break;

                case 2:
                    _playerSkills.Skill2PressUp1.Invoke();
                    break;

                case 3:
                    _playerSkills.Skill3PressUp1.Invoke();
                    break;

                case 4:
                    _playerSkills.Skill4PressUp1.Invoke();
                    break;
            }
        }
    }
    #endregion

    #region Vida
    //cuando detecta un cambio de vida en el servidor
    public void onHealthSyncPropertyChanged(float health)
    {
        if (_hPControl)
        {
            _hPControl.PlayerHealth = health;

            if (health <= 0)
            {
                destroySelf();
            }
            else
            {
                _hPControl.StartCoroutine(_hPControl.changeColor());

                if (GetComponentInChildren<HUDController>())
                    GetComponentInChildren<HUDController>().setHealthBar(_hPControl.PlayerHealth);

                if (GetComponentInChildren<HUDController>())
                    StartCoroutine(GetComponentInChildren<HUDController>().receiveDamageEffect());
            }
        }
    }

    //envia el cambio de vida al servidor
    public void changeHealth(float health)
    {

    }

    #endregion

    #region Fusion

    //Para agregar a la lista del host quimera
    public void pushAddMeToGeneralHost()
    {
        _photonView.RPC(ADD_ME_HOST, RpcTarget.Others);
    }

    [PunRPC]
    public void addMeToGeneralHost()
    {
        _fusionTrigger.AddMeToGeneralHost();
    }

    //Para sacar de la lista del host quimera
    public void pushGetoutToGeneralHost()
    {
        _photonView.RPC(GET_OUT_HOST, RpcTarget.Others);
    }

    [PunRPC]
    public void getoutToGeneralHost()
    {
        _fusionTrigger.GetoutToGeneralHost();
    }

    public GameObject createdChimeta()
    {
        return PhotonNetwork.Instantiate("Chimera", Vector3.zero, Quaternion.identity, 0);
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_hPControl.PlayerHealth);
            stream.SendNext(_playerMovement._flip);
        }
        else
        {
            onHealthSyncPropertyChanged((float)stream.ReceiveNext());
            _playerMovement._flip = (bool)stream.ReceiveNext();
        }
    }

}
