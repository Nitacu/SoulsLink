using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistAnimations : MonoBehaviour
{
    public float _damage = 20;
    private bool isColliding = false;
    private GameObject player;
    private bool isCharged = false;
    private float electricCost = 45;

    public void completeAnimation()
    {
        GetComponent<Animator>().SetBool("CreationCompleted", true);
    }

    public void setMist(float chargePercent, GameObject playerReference)
    {
        if(chargePercent >= electricCost)
        {
            isCharged = true;
            GetComponent<SpriteRenderer>().color = Color.cyan;
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0.45f;
            GetComponent<SpriteRenderer>().color = tmp;
            playerReference.GetComponent<Dash>().consumeChargeBar(electricCost);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (isCharged)
            {
                collision.gameObject.GetComponent<PolyNavAgent>().maxSpeed = collision.gameObject.GetComponent<PolyNavAgent>().maxSpeed / 2;
                collision.gameObject.GetComponent<PolyNavAgent>().velocity = collision.gameObject.GetComponent<PolyNavAgent>().velocity / 2;
            }
        }
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

        if (collision.CompareTag("Enemy"))
        {
            if (isCharged)
            {
                collision.gameObject.GetComponent<PolyNavAgent>().maxSpeed = collision.gameObject.GetComponent<PolyNavAgent>().maxSpeed * 2;
                collision.gameObject.GetComponent<PolyNavAgent>().velocity = collision.gameObject.GetComponent<PolyNavAgent>().velocity * 2;
            }
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
