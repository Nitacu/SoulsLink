using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using UnityEngine.InputSystem;

public class CharacterMultiplayerController : MonoBehaviour
{
    [SerializeField] private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;

    private PlayerMovement _playerMovement;
    private PlayerSkills _playerSkills;
    private PlayerAiming _playerAiming;

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
    private const string PLAYER_MOVEMENT = "playerMovement";
    private const string PLAYER_SKILLS = "playerSkills";
    private const string PLAYER_AIMING = "playerAiming";
    private const string PLAYER_START_POSITION= "loadStarPosition";
    #endregion


    private void Start()
    {
        if (!isMine())
        {
            Destroy(GetComponent<PlayerInput>());
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }

        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSkills = GetComponent<PlayerSkills>();
        _playerAiming = GetComponent<PlayerAiming>();
    }

    public bool isMine()
    {
        return _networkID.IsMine;
    }

    #region apuntar
    public void pushVectorAiming(Vector3 vector)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(vector);
        _remoteEventAgent.Invoke(PLAYER_AIMING, message);
    }

    public void playerAiming(SWNetworkMessage message)
    {
        _playerAiming.AimDirection = message.PopVector3();
    }
    #endregion

    #region skills
    //le envia a las otras maquinas los datos para que active las skills
    public void pushValueSkill(float value, int numberSkill)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(value);
        message.Push(numberSkill);
        _remoteEventAgent.Invoke(PLAYER_SKILLS, message);
    }

    // carga los datos y activa la skill correspondiente
    public void playerOnSkill(SWNetworkMessage message)
    {
        float value = message.PopFloat();
        int numeberSkill = message.PopInt16();

        if (value == 1)
        {
            switch (numeberSkill)
            {
                case 0:
                    _playerSkills.Skill1PressDown1.Invoke();
                    break;

                case 1:
                    _playerSkills.Skill2PressDown1.Invoke();
                    break;

                case 2:
                    _playerSkills.Skill3PressDown1.Invoke();
                    break;

                case 3:
                    _playerSkills.Skill4PressDown1.Invoke();
                    break;
            }
        }
        else if (value == 0)
        {
            switch (numeberSkill)
            {
                case 0:
                    _playerSkills.Skill1PressUp1.Invoke();
                    break;

                case 1:
                    _playerSkills.Skill2PressUp1.Invoke();
                    break;

                case 2:
                    _playerSkills.Skill3PressUp1.Invoke();
                    break;

                case 3:
                    _playerSkills.Skill4PressUp1.Invoke();
                    break;
            }
        }
    }
    #endregion 

    #region movimiento y posicion 

    //envia la posicion actiual del personaje
    private void pushPosition(Vector3 position)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(position);
        _remoteEventAgent.Invoke(PLAYER_START_POSITION, message);
    }

    //coloca el personaje en la posicion donde esta cuando entro a la sala
    public void loadStarPosition(SWNetworkMessage message)
    {
        transform.position = message.PopVector3();
    }

    //llama los metodos necesarios para mover la copia del player en las otras maquinas
    public void pushVectorMovement(Vector3 vector)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(vector);
        _remoteEventAgent.Invoke(PLAYER_MOVEMENT, message);
    }

    //Metodo donde llega el vector de movimiento y mueve al player de las otras maquinas
    public void playerMovement(SWNetworkMessage message)
    {
        _playerMovement.InputMovement= message.PopVector3();
    }
    #endregion
}
