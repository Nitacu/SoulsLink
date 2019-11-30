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

    [SerializeField] private GameObject _playerHUD;
    [SerializeField] private PhotonView _photonView;

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
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
        addDelegate();
    }

    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public void destroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void addDelegate()
    {
        _playerMovement._isMine = new PlayerMovement.DelegateMultiplayerController(isMine);

        _playerSkills._isMine = new PlayerSkills.DelegateMultiplayerController(isMine);
        _playerSkills._pushValueSkill = new PlayerSkills.DelegateMultiplayerControllerSendSkill(pushValueSkill);

        _playerAiming._isMine = new PlayerAiming.DelegateMultiplayerController(isMine);
        _playerAiming._pushVectorAiming = new PlayerAiming.DelegateMultiplayerControllerSendVector(pushVectorAiming);

        _hPControl._isMine = new PlayerHPControl.DelegateMultiplayerController(isMine);
        _hPControl._destroySelf = new PlayerHPControl.DelegateMultiplayerControllerDestroy(destroySelf);
        _hPControl._changeHealth = new PlayerHPControl.DelegateMultiplayerControllerHealth(changeHealth);
    }


    #region apuntar
    public void pushVectorAiming(Vector3 vector)
    {
        _photonView.RPC(PLAYER_AIMING,RpcTarget.Others, vector);
    }

    [PunRPC]
    public void playerAiming(Vector3 vector)
    {
        _playerAiming.AimDirection = vector;
    }
    #endregion

    #region skills
    //le envia a las otras maquinas los datos para que active las skills
    public void pushValueSkill(float value, float numberSkill)
    {
        _photonView.RPC(PLAYER_SKILLS,RpcTarget.Others,value,numberSkill);
    }

    // carga los datos y activa la skill correspondiente
    [PunRPC]
    public void playerOnSkill(float value, float numberSkill)
    {

        Debug.Log("valor " + value + " numero de skill " + numberSkill);

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

    //envia el cambio de vida al servidor
    public void changeHealth(float health)
    {

    }

    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_hPControl.PlayerHealth);
        }
        else
        {
            onHealthSyncPropertyChanged((float)stream.ReceiveNext());
        }
    }

}
