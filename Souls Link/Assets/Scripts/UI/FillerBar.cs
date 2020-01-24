using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FillerBar : MonoBehaviour
{
    [SerializeField] private Image _filler;

    void Start()
    {
        _filler.fillAmount = 0;
    }

    public void setFiller(float amount)
    {
        _filler.fillAmount = amount;
    }

}
