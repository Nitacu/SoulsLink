using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intangibility : Mist
{
    [SerializeField] private GameObject _wallCollider;

    public void makeStealth()
    {
        IsStealth = true;
        Color tmp = GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.1f;
        gameObject.GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color = tmp;
        gameObject.layer = LayerMask.NameToLayer("Invisible");

        GetComponent<Collider2D>().enabled = false;
        _wallCollider.SetActive(true);

        if (gameObject.GetComponent<PlayerHPControl>())
        {
            gameObject.GetComponent<PlayerHPControl>().setInmune(true);
        }
    }

    public override void disolveStealth()
    {
        base.disolveStealth();

        GetComponent<Collider2D>().enabled = true;
        _wallCollider.SetActive(false);

        if (gameObject.GetComponent<PlayerHPControl>())
        {
            gameObject.GetComponent<PlayerHPControl>().setInmune(false);
        }
    }
}
