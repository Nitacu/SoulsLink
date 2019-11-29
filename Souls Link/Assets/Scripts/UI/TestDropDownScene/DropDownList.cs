using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownList : MonoBehaviour
{
    List<string> options = new List<string>() { "Photon", "Socket" };
    private TMP_Dropdown _dropdown;
    // Start is called before the first frame update
    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        fillList();
    }

    private void fillList()
    {        
        _dropdown.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
