﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPControl : MonoBehaviour
{
    [SerializeField] private float _playerHealth;

    #region PLAYER STATES
    private bool canRecieveDamage = true;
    private bool deflectsDamage = false;


    #endregion

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    public delegate void DelegateMultiplayerControllerHealth(float health);
    public DelegateMultiplayerControllerHealth _changeHealth;
    public delegate void DelegateMultiplayerControllerDestroy();
    public DelegateMultiplayerControllerDestroy _destroySelf;
    #endregion

    public void recieveDamage(float damage, GameObject attackingEnemy)
    {
        if (_isMine())
        {
            if (canRecieveDamage)
            {
                PlayerHealth -= damage;
                //envia la modificacion a todos
                _changeHealth(PlayerHealth);

                if (PlayerHealth < 0 && _isMine())
                {
                    _destroySelf();
                }

                if (GetComponentInChildren<HUDController>())
                    GetComponentInChildren<HUDController>().setHealthBar(PlayerHealth);

                if (GetComponentInChildren<HUDController>())
                    StartCoroutine(GetComponentInChildren<HUDController>().receiveDamageEffect());

                StartCoroutine(changeColor());
            }
            else
            {
                if (deflectsDamage)
                {
                    attackingEnemy.GetComponent<SimpleEnemyController>().recieveDamage(damage);
                }
            }
        }
    }

    public void setReflectiveMode()
    {
        canRecieveDamage = false;
        deflectsDamage = true;
    }

    public void setNormalMode()
    {
        canRecieveDamage = true;
        deflectsDamage = false;
    }

    public IEnumerator changeColor()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        canRecieveDamage = false;
        Debug.Log("canRecieveDamage = false");

        yield return new WaitForSeconds(0.5f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        canRecieveDamage = true;
        Debug.Log("canRecieveDamage = true");
    }

    /// /////////////////////////////////////GET Y SET /////////////////////////////////////
    public float PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
}
