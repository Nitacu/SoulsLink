using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaRing : MonoBehaviour
{
    [SerializeField] private GameObject _ringPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _damage;
    [SerializeField] private float _numberOfRings;
    [SerializeField] private float _spawnRingRate;
    
    private float _coolDownTracker;
    private PlayerAiming _aiming;
    private float ringsSpawned = 0;
    private bool lastRing = false;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
    }

    private void Update()
    {

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }




        /*
        if (_skillMaster.SkillTrigger.skill1.pressedDown && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
        */
    }

    public void pressKey()
    {
        if(_coolDownTracker <= 0)
        {
            spawnRing();
            disableMovement();
        }
    }

    public void disableMovement()
    {
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    public void setBackToNormal()
    {
        ringsSpawned = 0;
        lastRing = false;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }

    public void stopShooting()
    {
        shooting = false;
    }

    private bool shooting = false;
    public void startShoot()
    {
        shooting = true;
    }

    public void spawnRing()
    {
        //if (_coolDownTracker <= 0)
        //{
        ringsSpawned++;
        if(ringsSpawned == _numberOfRings)
        {
            lastRing = true;
        }
        Vector2 direction = _aiming.AimDirection;
        _coolDownTracker = _coolDown;
        Vector2 newPos = Vector2.zero;
        newPos.y = newPos.y - 0.2f;
        GameObject ring = Instantiate(_ringPrefab, gameObject.transform);
        ring.transform.localPosition = newPos;
        ring.GetComponent<MagmaRingController>().setMagma(_damage, gameObject, lastRing);
        if(ringsSpawned < _numberOfRings)
        {
            StartCoroutine(spawnAnotherRing(_spawnRingRate));
        }
        //}
    }

    IEnumerator spawnAnotherRing(float time)
    {
        yield return new WaitForSeconds(time);
        spawnRing();
    }
}
