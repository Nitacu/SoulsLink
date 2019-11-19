using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{

    [SerializeField] private float _distance = 1;
    [SerializeField] private GameObject _decoyShadow;

    [SerializeField] private float _coolDown;
    private float _coolDownTracker;

    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
    }

    private void Update()
    {


        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }
    }

    public void pressKey()
    {
        if (_coolDownTracker <= 0)
        {
            decoyTp();
            _coolDownTracker = _coolDown;
        }
    }

    private void decoyTp()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = (Vector2)transform.position + (_aiming.AimDirection * _distance);

        transform.position = newPos;
        GameObject decoyShadow = Instantiate(_decoyShadow);
        decoyShadow.transform.position = currentPos;
    }
}
