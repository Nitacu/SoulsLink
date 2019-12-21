﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : Skill
{
    
    public GameObject _wallPrefab;
    public float _speed = 6f;
    private float _coolDownTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
        _coolDownTracker = _coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

    }

    public void spawnGroundSmash()
    {
        if (_coolDownTracker <= 0)
        {
            Vector2 direction = GetComponent<PlayerAiming>().AimDirection;
            _coolDownTracker = _coolDown;
            GameObject temp = Instantiate(_wallPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<MovingWallController>().setWall(direction, _speed);
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            temp.transform.rotation= Quaternion.Euler(0, 0, rot);
        }
    }

    private void stopMoving()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
    }

    public void backToNormal()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }
}
