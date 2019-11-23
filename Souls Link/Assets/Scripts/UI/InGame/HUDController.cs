using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using SWNetwork;

public class HUDController : MonoBehaviour
{
    const string DAMAGE_RECEIVE = "DamageReceived";

    [SerializeField] TextMeshProUGUI _multiplayerState;

    [SerializeField] private Animator _receivDamageAnim;

    [SerializeField] private Image _healthBar;

    private float maxPlayerHealth;

    private NetworkClient networkInfo;
    private bool isHost = false;

    private void OnEnable()
    {
        maxPlayerHealth = GetComponentInParent<PlayerHPControl>().PlayerHealth;
        setHealthBar(maxPlayerHealth);
        setMultiplayerConnectionType();

        StartCoroutine(findNetworkClientAgain(0));

    }

    private void Update()
    {
        setMultiplayerConnectionType();
    }

    public void setMultiplayerConnectionType()
    {
        if (networkInfo != null)
        {
            if (isHost != networkInfo.IsHost)
            {
                setMultiplayerTextType();
                isHost = networkInfo.IsHost;
            }
        }      
    }

    private void setMultiplayerTextType()
    {
        _multiplayerState.text = (networkInfo.IsHost) ? "Host" : "Invited";
    }

    IEnumerator findNetworkClientAgain(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        networkInfo = FindObjectOfType<NetworkClient>();

        if (networkInfo == null)
        {
            StartCoroutine(findNetworkClientAgain(2));
        }
        else
        {
            setMultiplayerTextType();
        }
    }

    public void setHealthBar(float newPlayerHealth)
    {
        float healthFactor = newPlayerHealth / maxPlayerHealth;

        healthFactor = Mathf.Clamp(healthFactor, 0, healthFactor);

        _healthBar.fillAmount = healthFactor;
    }

    public IEnumerator receiveDamageEffect()
    {
        if (_receivDamageAnim != null)
        {
            Debug.Log("Set Animation on: " + true);
            _receivDamageAnim.SetBool(DAMAGE_RECEIVE, true);

            yield return new WaitForSeconds(0.5f);

            Debug.Log("Set Animation on: " + true);
            _receivDamageAnim.SetBool(DAMAGE_RECEIVE, false);
        }
    }


}
