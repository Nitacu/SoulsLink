using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using SWNetwork;
using Photon.Pun;

public class HUDController : MonoBehaviour
{
    const string DAMAGE_RECEIVE = "DamageReceived";

    [SerializeField] TextMeshProUGUI _multiplayerState;

    [SerializeField] private Animator _receivDamageAnim;

    [SerializeField] private Image _healthBar;

    private float maxPlayerHealth;

    private bool isHost = false;

    private GameManager.MultiplayerServer _server;
    private bool _isHostServer = false;

    private void OnEnable()
    {
        _server = GameManager.GetInstace()._multiplayerServer;
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
        if (connet())
        {
            if (isHost != defineHost())
            {
                setMultiplayerTextType();
                isHost = _isHostServer;
            }
        }
    }

    private void setMultiplayerTextType()
    {
        _multiplayerState.text = (_isHostServer) ? "Host" : "Invited";
    }

    IEnumerator findNetworkClientAgain(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        if (connet())
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

    private bool defineHost()
    {
        switch (_server)
        {
            case GameManager.MultiplayerServer.SOCKETWEAVER:
                _isHostServer = NetworkClient.Instance.IsHost;
                break;

            case GameManager.MultiplayerServer.PHOTON:
                _isHostServer = PhotonNetwork.IsMasterClient;
                break;
        }

        return _isHostServer;
    }

    private bool connet()
    {
        switch (_server)
        {
            case GameManager.MultiplayerServer.SOCKETWEAVER:
                return NetworkClient.Instance.IsHost;

            case GameManager.MultiplayerServer.PHOTON:
                return PhotonNetwork.IsConnected;

            default:
                return false;

        }
    }
}
