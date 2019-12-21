using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashAttack : Skill
{
    private GameObject chargeBar;
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _damage;
    public float _knockBackDuration = 1;
    private float forceUsed = 0;
    private GameObject _attackReference;
    private bool isCharging = false;
    private float chargedTime = 0;
    public float maxChargedSeconds = 3;
    private Vector2 attackDirection;
    public float attackDuration = 0.2f;
    private bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }

        chargeBarControl();

        if (isCharging)
        {
            chargedTime += Time.deltaTime;
        }
    }

    private void stopMoving()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
    }

    private void backToNormal()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }

    public void setSmashBar(GameObject smashBar)
    {
        chargeBar = smashBar;
    }


    private void findForce(float _pressedTime)
    {
        if (_pressedTime > maxChargedSeconds)
        {
            _pressedTime = maxChargedSeconds;
        }

        float pressedTimePercent = (_pressedTime * 100) / maxChargedSeconds;
        float distanceDifference = _maxForce - _minForce;


        forceUsed = ((pressedTimePercent * distanceDifference) / 100) + _minForce;


    }

    public void pressKey()
    {
        if (CoolDownTracker <= 0)
        {
            if (!IsCasting)
            {
                isCharging = true;
                stopMoving();
            }
            else
            {
                smashAttack();
            }

            
        }
    }

    public void unPressKey()
    {
        if (CoolDownTracker <= 0)
        {
            if (!IsCasting)
            {
                
                isCharging = false;
                IsCasting = true;
                findForce(chargedTime);
                backToNormal();

            }
        }

    }

    private void smashAttack()
    {
        
        captureDirection();
        chargedTime = 0;
        IsCasting = false;
        CoolDownTracker = _coolDown;
        _attackReference = Instantiate(_attackPrefab, gameObject.transform);
        _attackReference.GetComponentInChildren<SmashController>().setSmash(attackDirection, forceUsed, _damage, _knockBackDuration);
        _attackReference.transform.localPosition = new Vector2(0, 0);
        StartCoroutine(destroyAttack(attackDuration, _attackReference));
    }

    IEnumerator destroyAttack(float time, GameObject attack)
    {
        yield return new WaitForSeconds(time);
        Destroy(attack);
    }

    private void chargeBarControl()
    {
        if (chargedTime > maxChargedSeconds)
        {
            chargedTime = maxChargedSeconds;
        }
        chargeBar.GetComponent<Image>().fillAmount = ((100 * chargedTime) / maxChargedSeconds) / 100;
    }

    private void captureDirection()
    {
        Vector2 newDirection = GetComponent<PlayerAiming>().AimDirection;
        attackDirection = newDirection;
    }
}
