using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject _flameThrowerParticles;
    [SerializeField] private GameObject _colliderFire;
    [SerializeField] private float _coolDown;
    [SerializeField] private float flameDuration;
    [SerializeField] private float _damagePerTick;
    [SerializeField] private float _timeTicks;
    [SerializeField] private KeyCode _inputAttack;
    private bool isShooting = false;
    private float _coolDownTracker;
    private GameObject flame;
    private GameObject colliderFlame;
    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
    }

    private void Update()
    {

        if (isShooting)
        {
            float rot = Mathf.Atan2(GetComponent<AimCursor>().LastVector.normalized.x, GetComponent<AimCursor>().LastVector.normalized.y) * Mathf.Rad2Deg;
            flame.transform.rotation = Quaternion.Euler(rot - 90, 90, 90);
            float rot2 = Mathf.Atan2(GetComponent<AimCursor>().LastVector.normalized.y, GetComponent<AimCursor>().LastVector.normalized.x) * Mathf.Rad2Deg;
            colliderFlame.transform.rotation = Quaternion.Euler(0, 0, rot2);
        }

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

        if (Input.GetKeyDown(_inputAttack) && _coolDownTracker <= 0)
        {
            spawnFire();
            isShooting = true;
        }
       

        /*
        if (_skillMaster.SkillTrigger.skill1.pressedDown && _coolDownTracker <= 0)
        {
            shotStake(_aiming.AimVector.normalized);
            _coolDownTracker = _coolDown;
        }
        */
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

    public void spawnFire()
    {
        //if (_coolDownTracker <= 0)
        //{
        Vector2 direction = GetComponent<AimCursor>().LastVector.normalized;
        _coolDownTracker = _coolDown;
        flame = Instantiate(_flameThrowerParticles, gameObject.transform);
        colliderFlame = Instantiate(_colliderFire, gameObject.transform);
        colliderFlame.GetComponent<FlameControl>().setFlame(_damagePerTick,_timeTicks, flameDuration);
        StartCoroutine(destroyFire(flame, flameDuration, colliderFlame));
        flame.transform.position = gameObject.transform.position;
        
        //}
    }

    IEnumerator destroyFire(GameObject fire, float time, GameObject colliderF)
    {
        yield return new WaitForSeconds(time);
        isShooting = false;
        Destroy(colliderF);
        Destroy(fire);
    }
}
