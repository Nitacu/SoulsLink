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

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
    private const string PLAYER_MOVEMENT = "playerMovement";
    private const string PLAYER_SKILLS = "playerSkills";
    #endregion


    private void Start()
    {
        if (!isMine())
            Destroy(GetComponent<PlayerInput>());

        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSkills = GetComponent<PlayerSkills>();
    }

    public bool isMine()
    {
        Debug.Log("El ID es " + _networkID.NetworkObjectId);
        return _networkID.IsMine;
    }

    public void pushValueSkill(float value, int numberSkill)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(value);
        message.Push(numberSkill);
        _remoteEventAgent.Invoke(PLAYER_SKILLS, message);
    }

    public void playerOnSkill(SWNetworkMessage message)
    {
        float value = message.PopFloat();
        int numeberSkill = message.PopInt16();

        if (value == 1)
        {
            switch (numeberSkill)
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
            }
        }
        else if (value == 0)
        {
            switch (numeberSkill)
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
        Debug.Log("me llego algo");
        _playerMovement.InputMovement = message.PopVector3();
        GetComponent<SpriteRenderer>().color = Color.red;
    }

}
