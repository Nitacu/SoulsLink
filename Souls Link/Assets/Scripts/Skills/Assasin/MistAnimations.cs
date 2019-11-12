using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistAnimations : MonoBehaviour
{
    private bool isColliding = false;
    private GameObject player;

    public void completeAnimation()
    {
        GetComponent<Animator>().SetBool("CreationCompleted", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<Mist>().activateInsideMist();
        }
        isColliding = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Mist>().resetStealth();
            collision.gameObject.GetComponent<Mist>().IsStealth = true;
            isColliding = false;
        }
    }

    private void OnDisable()
    {
        if (isColliding)
        {
            player.GetComponent<Mist>().resetStealth();
            player.GetComponent<Mist>().IsStealth = true;
        }
    }
}
