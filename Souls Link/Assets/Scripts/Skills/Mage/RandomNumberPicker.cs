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
            StartCoroutine(pickGun(finalNumberShowTime));
        }

    }


    IEnumerator pickGun(float finalTime)
    {
        yield return new WaitForSeconds(finalTime);
        _positionWithPlayerReference.GetComponent<RandomGunChooser>().recieveGunData(randomNumber);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
