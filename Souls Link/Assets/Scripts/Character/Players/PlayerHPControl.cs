﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPControl : MonoBehaviour
{
    [SerializeField] private float _playerHealth;
    private float maxPlayerHealth;
    public float MaxPlayerHealth
    {
        get { return maxPlayerHealth; }
    }

    #region PLAYER STATES
    private bool canRecieveDamage = true;
    private bool deflectsDamage = false;
    private bool stunDeflect = false;
    private bool withAbsorbShield = false;
    #endregion

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    public delegate void DelegateMultiplayerControllerDestroy();
    public DelegateMultiplayerControllerDestroy _destroySelf;
    #endregion

    private void Start()
    {
        maxPlayerHealth = _playerHealth;
    }

    public void setInitialHealth(float health)
    {
        _playerHealth = health;
        maxPlayerHealth = _playerHealth;

    }

    public void recieveDamage(float damage, GameObject attackingEnemy)
    {
        if (_isMine())
        {
            if (canRecieveDamage)
            {
                PlayerHealth -= damage;

                if (PlayerHealth < 0 && _isMine())
                {
                    PlayerHealth = 0;

                    //Die
                    if (gameObject.GetComponent<ChimeraController>())//Die as chimera
                    {
                        dieAsChimera();
                    }
                    else if (gameObject.GetComponent<PlayerMovement>())//Die as player
                    {
                        

                        dieAsPlayer();
                    }
                    //_destroySelf();
                }

                if (GetComponentInChildren<HUDController>())
                    GetComponentInChildren<HUDController>().setHealthBar(PlayerHealth);

                if (GetComponentInChildren<HUDController>())
                    StartCoroutine(GetComponentInChildren<HUDController>().receiveDamageEffect());

                StartCoroutine(changeColor());
            }
            else
            {
                if (withAbsorbShield)
                {
                    if (GetComponent<ExplosiveShield>())
                    {
                        GetComponent<ExplosiveShield>().absorbDamage(damage);
                    }
                }

                if (deflectsDamage)
                {
                    if (attackingEnemy.tag != "enemyProjectile")
                    {
                        attackingEnemy.GetComponent<SimpleEnemyController>().recieveDamage(damage);
                    }
                }

                if (stunDeflect)
                {
                    if (attackingEnemy.tag != "enemyProjectile")
                    {
                        attackingEnemy.GetComponent<SimpleEnemyController>().Stun(2);
                    }
                }
            }
        }
    }

    public void dieAsPlayer()
    {
        if (GetComponent<Resurrection>())
        {
            GetComponent<Resurrection>().setResurrection(false);
        }
        else
        {
            gameObject.SetActive(false);

            GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();

            if (sceneManager != null)
            {
                sceneManager.setCoopCamera(false);
            }
        }
    }

    public void dieAsChimera()
    {
        gameObject.GetComponent<ChimeraController>().chimeraDie();
    }

    //las funciones del photon llaman a este metodo en las otras maquinas
    public void updateMultiplayerHealth(float playerHealth)
    {
        if (playerHealth != _playerHealth)
        {
            _playerHealth = playerHealth;
            StartCoroutine(changeColor());
        }
    }

    public void healHP(float heal)
    {
        PlayerHealth += heal;
        if (PlayerHealth > maxPlayerHealth)
        {
            PlayerHealth = maxPlayerHealth;
        }
        GetComponentInChildren<SpriteRenderer>().color = Color.green;
        Invoke("backToWhiteColor", 0.5f);
        if (GetComponentInChildren<HUDController>())
            GetComponentInChildren<HUDController>().setHealthBar(PlayerHealth);

    }

    private void backToWhiteColor()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    public void setReflectiveMode()
    {
        canRecieveDamage = false;
        deflectsDamage = true;
    }

    public void setInmune(bool wantInmune)
    {
        canRecieveDamage = !wantInmune;
    }

    public void setStunReflectMode()
    {
        canRecieveDamage = false;
        deflectsDamage = true;
        stunDeflect = true;
    }

    public void setNormalMode()
    {
        canRecieveDamage = true;
        deflectsDamage = false;
        stunDeflect = false;
    }

    public void setAbsorbShield(bool withShield)
    {
        canRecieveDamage = !withShield;
        withAbsorbShield = withShield;
    }


    public IEnumerator changeColor()
    {
        if (GetComponentInChildren<SpriteRenderer>())
            GetComponentInChildren<SpriteRenderer>().color = Color.red;

        canRecieveDamage = false;

        yield return new WaitForSeconds(0.5f);
        if (GetComponentInChildren<SpriteRenderer>())
            GetComponentInChildren<SpriteRenderer>().color = Color.white;

        canRecieveDamage = true;
    }

    /// /////////////////////////////////////GET Y SET /////////////////////////////////////
    public float PlayerHealth { get => _playerHealth; set => _playerHealth = value; }


    //PARA TESTING --BORRAR--
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            recieveDamage(30f, null);
        }
    }
}
