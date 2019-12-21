using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

public class HUDController : MonoBehaviour
{
    const string DAMAGE_RECEIVE = "DamageReceived";

    [SerializeField] private Animator _receivDamageAnim;

    [SerializeField] private Image _healthBar;

    private float maxPlayerHealth;

    private bool isHost = false;

    private bool _isHostServer = false;

    private void Start()
    {
        maxPlayerHealth = GetComponentInParent<PlayerHPControl>().PlayerHealth;
        setHealthBar(maxPlayerHealth);
    }

    public void setHealthBar(float newPlayerHealth)
    {
        if (newPlayerHealth > maxPlayerHealth)
        {
            newPlayerHealth = maxPlayerHealth;
        }
        float healthFactor = newPlayerHealth / maxPlayerHealth;

        healthFactor = Mathf.Clamp(healthFactor, 0, healthFactor);

        _healthBar.fillAmount = healthFactor;
    }

    public IEnumerator receiveDamageEffect()
    {
        yield return new WaitForSeconds(0);

        /*
        if (_receivDamageAnim != null)
        {
            Debug.Log("Set Animation on: " + true);
            _receivDamageAnim.SetBool(DAMAGE_RECEIVE, true);

            yield return new WaitForSeconds(0.5f);

            Debug.Log("Set Animation on: " + true);
            _receivDamageAnim.SetBool(DAMAGE_RECEIVE, false);
        }
        */
    }

    private bool defineHost()
    {
        _isHostServer = PhotonNetwork.IsMasterClient;

        return _isHostServer;
    }

    private bool connet()
    {
        return PhotonNetwork.IsConnected;
    }



}
