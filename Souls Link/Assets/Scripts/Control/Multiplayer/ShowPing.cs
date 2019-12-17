using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ShowPing : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void FixedUpdate()
    {
        text.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }
}
