using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float health = 100;
    public float _time;
    public float indexInArray = 0;
    public GameObject playerReference;

    public void setDecoy(GameObject _playerReference)
    {
        playerReference = _playerReference;
    }

    public void loseHealth(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            StartCoroutine(autoDestroy(0.1f));
        }
    }

    private void Start()
    {
        StartCoroutine(autoDestroy(_time));
    }

    IEnumerator autoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        playerReference.GetComponent<Decoy>().clones.Remove(gameObject);
        playerReference.GetComponent<Decoy>().contClones--;
        Destroy(gameObject);
    }
}
