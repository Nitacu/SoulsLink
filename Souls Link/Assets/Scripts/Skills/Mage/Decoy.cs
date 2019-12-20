using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : Skill
{

    [SerializeField] private float _distance = 1;
    [SerializeField] private GameObject _decoyShadow;


    private PlayerAiming _aiming;
    [HideInInspector]
    public List<GameObject> clones = new List<GameObject>();
    [HideInInspector]
    public float contClones;
    private float isEmpty = 0;
    private bool oneEmpty = false;

    protected GameObject decoyShadow;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
    }

    private void Update()
    {


        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
    }

    public void pressKey()
    {
        if (CoolDownTracker <= 0)
        {
            decoyTp();
            CoolDownTracker = _coolDown;
        }
    }

    protected virtual void decoyTp()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = (Vector2)transform.position + (_aiming.AimDirection * _distance);
        
        //transform.position = newPos;
        if (contClones < 3)
        {
            contClones++;
            decoyShadow = Instantiate(_decoyShadow);
            decoyShadow.GetComponent<SelfDestroy>().setDecoy(gameObject);
            clones.Add(decoyShadow);
            decoyShadow.transform.position = currentPos;
        }
        else
        {
            decoyShadow = Instantiate(_decoyShadow);
            decoyShadow.GetComponent<SelfDestroy>().setDecoy(gameObject);
            Destroy(clones[0]);
            clones.RemoveAt(0);
            clones.Add(decoyShadow);
            decoyShadow.transform.position = currentPos;
        }
    }
}
