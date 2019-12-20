using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaRing : Skill
{
    [SerializeField] private GameObject _ringPrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _numberOfRings;
    [SerializeField] private float _spawnRingRate;
    [SerializeField] private float _magmaTimeAlive;

    private PlayerAiming _aiming;
    private float ringsSpawned = 0;
    private bool lastRing = false;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        CoolDownTracker = _coolDown;
    }

    private void Update()
    {

        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
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
        if(CoolDownTracker <= 0)
        {
            spawnRing();            
        }
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
        CoolDownTracker = _coolDown;        
        GameObject ring = Instantiate(_ringPrefab, gameObject.transform.position, Quaternion.identity);       
        ring.GetComponent<MagmaRingController>().setMagma(_damage, gameObject, lastRing, _magmaTimeAlive);
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
