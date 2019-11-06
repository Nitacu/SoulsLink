using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float _time;

    private void Start()
    {
        StartCoroutine(autoDestroy());
    }

    IEnumerator autoDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
