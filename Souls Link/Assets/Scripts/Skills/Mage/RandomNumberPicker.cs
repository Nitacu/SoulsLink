using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomNumberPicker : MonoBehaviour
{
    private float totalRandomizerNumbers = 10;
    private float contRandomizer = 0;
    private float timeBetweenNumbers = 0.2f;
    private float randomNumber = 0;
    private float _numberWeapon;
    private float finalNumberShowTime = 1.3f;

    private GameObject _positionWithPlayerReference;

    public void setRandomizer(GameObject reference)
    {
        _positionWithPlayerReference = reference;
    }

    public void randomize()
    {
        contRandomizer++;
        StartCoroutine(switchPickerNumber(timeBetweenNumbers));
    }

    IEnumerator switchPickerNumber(float time)
    {
        yield return new WaitForSeconds(time);
        if(contRandomizer < totalRandomizerNumbers)
        {
            randomNumber = Random.Range(1, 9);
            GetComponentInChildren<TextMeshProUGUI>().text = randomNumber.ToString();
            randomize();
        }
        else
        {
            //cuando termina el tiempo de la ruleta coloca el arma que previamente se habia seleccionado 
            GetComponentInChildren<TextMeshProUGUI>().text = NumberWeapon.ToString();
            StartCoroutine(pickGun(finalNumberShowTime));
        }

    }


    IEnumerator pickGun(float finalTime)
    {
        yield return new WaitForSeconds(finalTime);
        _positionWithPlayerReference.GetComponent<RandomGunChooser>()._photonView.RPC("recieveGunData", Photon.Pun.RpcTarget.Others, NumberWeapon);
        _positionWithPlayerReference.GetComponent<RandomGunChooser>().recieveGunData(NumberWeapon);
        NumberWeapon = 0;
        Destroy(gameObject);
    }


    /// ///////////////////////////GET Y SET /////////////////////////////////////
    public float NumberWeapon { get => _numberWeapon; set => _numberWeapon = value; }
}
