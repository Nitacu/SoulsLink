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
            if (player.GetComponent<Mist>() != null)
            {
                player.GetComponent<Mist>().activateInsideMist();
            }
            else
            {
                Color tmp = player.GetComponentInChildren<SpriteRenderer>().color;
                tmp.a = 0.1f;
                player.GetComponentInChildren<SpriteRenderer>().color = tmp;
                player.layer = LayerMask.NameToLayer("Invisible");
            }
        }
        isColliding = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Mist>() != null)
            {
                collision.gameObject.GetComponent<Mist>().resetStealth();
                collision.gameObject.GetComponent<Mist>().IsStealth = true;
            }
            else
            {
                Color tmp = collision.gameObject.GetComponentInChildren<SpriteRenderer>().color;
                tmp.a = 1f;
                collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = tmp;
                collision.gameObject.layer = LayerMask.NameToLayer("Player");
            }
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
