using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStartController : MonoBehaviour
{
    public GameObject entrance;

    [HideInInspector]
    public bool isSteppingOnEntrance = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<ChimeraController>() != null)
            {
                isSteppingOnEntrance = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            isSteppingOnEntrance = false;

        }
    }
}
