﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using UnityEngine.InputSystem;

public class CharacterMultiplayerController : MonoBehaviour
{
    [SerializeField] private NetworkID _networkID;
    [SerializeField] private RemoteEventAgent _remoteEventAgent;
    [SerializeField] private SyncPropertyAgent _syncPropertyAgent;
    [SerializeField] private GameObject _playerHUD;


    private PlayerMovement _playerMovement;
    private PlayerSkills _playerSkills;
    private PlayerAiming _playerAiming;
    private PlayerHPControl _hPControl;

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
    private const string PLAYER_SKILLS = "playerSkills";
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
    }

    public bool isMine()
    {
        return _networkID.IsMine;
    }

    public void destroySelf()
    {
        NetworkClient.Destroy(gameObject);
    }

    #region Flip
    //inicializa la vida
    public void onFlipSyncPropertyReady()
    {
        bool flip = _syncPropertyAgent.GetPropertyWithName(FLIP).GetBoolValue();

        if (isMine())
        {
            int version = _syncPropertyAgent.GetPropertyWithName(FLIP).version;

            if (version == 0)
            {
                // colocar la vida en el maximo
                _syncPropertyAgent.Modify(FLIP, _playerMovement.Renderer.flipX);
                flip = _playerMovement.Renderer.flipX;
            }
        }

        // carga la vida
        _playerMovement.Renderer.flipX = flip;
    }

    //cuando detecta un cambio de vida en el servidor
    public void onFlipSyncPropertyChanged()
    {
        bool flip = _syncPropertyAgent.GetPropertyWithName(FLIP).GetBoolValue();

        _playerMovement.Renderer.flipX = flip;
    }

    //envia el cambio de vida al servidor
    public void changeFlip(bool flip)
    {
        _syncPropertyAgent.Modify(FLIP, flip);
    }

    #endregion

    #region Vida
    //inicializa la vida
    public void onHealthSyncPropertyReady()
    {
        float health = _syncPropertyAgent.GetPropertyWithName(HEALTH).GetFloatValue();

        if (isMine())
        {
            int version = _syncPropertyAgent.GetPropertyWithName(HEALTH).version;

            if (version == 0)
            {
                // colocar la vida en el maximo
                _syncPropertyAgent.Modify(HEALTH, _hPControl.PlayerHealth);
                health = _hPControl.PlayerHealth;
            }
        }

        // carga la vida
        _hPControl.PlayerHealth = health;
    }

    //cuando detecta un cambio de vida en el servidor
    public void onHealthSyncPropertyChanged()
    {
        float health = _syncPropertyAgent.GetPropertyWithName(HEALTH).GetFloatValue();
        _hPControl.PlayerHealth = health;

        if (health <= 0 && _syncPropertyAgent.GetPropertyWithName(HEALTH).version > 1)
        {
            destroySelf();
        }
        else if(_syncPropertyAgent.GetPropertyWithName(HEALTH).version > 1)
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
        _syncPropertyAgent.Modify(HEALTH, health);
    }

    #endregion

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
    public void pushValueSkill(float value, float numberSkill)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(value);
        message.Push(numberSkill);
        Debug.Log("valor " + value + " numero de skill "+ numberSkill);
        _remoteEventAgent.Invoke(PLAYER_SKILLS, message);
    }

    // carga los datos y activa la skill correspondiente
    public void playerOnSkill(SWNetworkMessage message)
    {
        float value = message.PopFloat();
        float numeberSkill = message.PopFloat();

        Debug.Log("valor " + value + " numero de skill " + numeberSkill);

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
                case 5:
                    _playerSkills.DashPressDown.Invoke();
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
    #endregion 

}
