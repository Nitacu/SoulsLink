using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : Skill
{
    private Vector2 dashDirection;
    private GameObject chargeBar;
    private GameObject dashEffect;
    [SerializeField]
    private GameObject chargeEffect;
    private Rigidbody2D _rb;
    [Header("General Settings")]
    public float dashDuration = 0;
    private float durationTracker = 0;
    public float _damage = 0;
    private CircleCollider2D _collider;
    public GameObject _dashCollider;
    [Header("Simple Dash Settings")]
    public float dashSpeed = 0;
    private GameObject effectReference;
    


    private PlayerAiming _aiming;
    public PlayerAiming Aiming { get => _aiming; set => _aiming = value; }
    public bool isDashing = false;

    [Header("Charged Dash Settings")]
    public bool chargedDash = false;
    private float chargedTime = 0;
    public float maxChargedSeconds = 0;
    public float maxDashSpeed = 0;
    public float minDashSpeed = 0;
    public float maxDuration = 0;
    public float minDuration = 0;

    [Header("MultiDashing")]
    public bool canMultiDash = false;
    public float maxDashes = 0;
    private float currentDashes = 0;

    private bool isCharging = false;
    private bool canDash = false;
    private bool hasCharged = false;
    [HideInInspector]
    public float chargePercent = 0;
    private float electricCost = 20;
    [HideInInspector]
    public bool isSimpleDash = false;

    // Start is called before the first frame update
    void Start()
    {
        dashEffect = GetComponentInChildren<TrailRenderer>().gameObject;
        dashEffect.SetActive(false);
        _dashCollider.SetActive(false);
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        CoolDownTracker = _coolDown;
        durationTracker = dashDuration;
        _aiming = GetComponent<PlayerAiming>();
        chargePercent = 0;
    }

    public void setChargeBar(GameObject _chargeBar)
    {
        chargeBar = _chargeBar;
        chargeBar.GetComponent<Image>().fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!chargedDash)
        {
            simpleDash();
        }
        else
        {
            chargeDash();
            chargeBarControl();
        }

        if (isCharging)
        {
            chargedTime += Time.deltaTime;
            if(chargedTime > maxChargedSeconds)
            {
                isCharging = false;
                backToNormal();
                getChargePercent(chargedTime);
                hasCharged = true;
                Destroy(effectReference);
                GetComponent<PlayerHPControl>().setNormalMode();
            }
        }
    }

    public void playerDash(Vector2 direction)
    {
        PlayerMovement playerMovementRef = GetComponent<PlayerMovement>();

        playerMovementRef.RigidBodyPlayer.velocity = dashDirection * dashSpeed;
        playerMovementRef.enabled = true;
        //Debug.Log(direction);
    }

    private void chargeBarControl()
    {
        if (chargedTime > maxChargedSeconds)
        {
            chargedTime = maxChargedSeconds;
        }
        if (chargedTime <= 0)
        {
            chargedTime = 0;
        }
        chargeBar.GetComponent<Image>().fillAmount = ((100 * chargedTime) / maxChargedSeconds) / 100;
    }

    public void consumeChargeBar(float percent)
    {
        float numberToDiscount = (percent * maxChargedSeconds) / 100;

        chargedTime = chargedTime - numberToDiscount;
        chargePercent = chargePercent - percent;
        chargeBarControl();

        getChargePercent(chargedTime);

    }

    private void captureDirection()
    {
        Vector2 newDirection = GetComponent<PlayerAiming>().AimDirection;
        dashDirection = newDirection;
    }

    private void chargeDash()
    {
        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
        if (CoolDownTracker <= 0)
        {
            chargedDashCheck();
        }
    }

    private void simpleDash()
    {
        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
        if (CoolDownTracker <= 0)
        {

        }
    }

    private void chargedDashCheck()
    {
        if (!isDashing)
        {
            _dashCollider.SetActive(false);
        }
        else
        {
            _dashCollider.SetActive(true);
            if (durationTracker <= 0)
            {
                //Dash is over
                isDashing = false;
                isCharging = false;
                canDash = false;
                dashEffect.SetActive(false);
                durationTracker = dashDuration;
                CoolDownTracker = _coolDown;

                GetComponent<PlayerMovement>().isDashing = false;
            }
            else
            {
                //Dashing
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMovement>().isDashing = true;

            }
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

    private void findDashSpeed(float _pressedTime, float _dashDuration)
    {
        if (_pressedTime > maxChargedSeconds)
        {
            _pressedTime = maxChargedSeconds;

        }

        _pressedTime = maxChargedSeconds;


        float pressedTimePercent = (_pressedTime * 100) / maxChargedSeconds;
        float distanceDifference = maxDashSpeed - minDashSpeed;
        float durationDifference = maxDuration - minDuration;

        dashDuration = ((pressedTimePercent * durationDifference) / 100) + minDuration;
        durationTracker = dashDuration;
        dashSpeed = ((pressedTimePercent * distanceDifference) / 100) + minDashSpeed;
    }

    public void dashKey()
    {
        if (chargePercent >= electricCost)
        {
            //dash with power and does damage
            isSimpleDash = false;
            consumeChargeBar(electricCost);
            isCharging = false;
            hasCharged = false;
            dashEffect.SetActive(true);
            captureDirection();
            findDashSpeed(chargedTime, dashDuration);
            isDashing = true;
        }
        else
        {
            isSimpleDash = true;
            // dash normally with no damage
            GetComponent<CometDash>().pressKey();
        }
    }

    public void newPressKey()
    {
        if (chargePercent < 99)
        {
            //charge
            canDash = true;
            isCharging = true;
            stopMoving();
            effectReference = Instantiate(chargeEffect, gameObject.transform);
            GetComponent<PlayerHPControl>().setStunReflectMode();
        }
        
    }


    public void newUnpress()
    {
        if (isCharging)
        {
            isCharging = false;
            backToNormal();
            getChargePercent(chargedTime);
            hasCharged = true;
            Destroy(effectReference);
            GetComponent<PlayerHPControl>().setNormalMode();
        }
    }

    public void pressKey()
    {

        if (CoolDownTracker <= 0)
        {
            canDash = true;
            if (chargedDash)
            {
                if (!isDashing && hasCharged && chargePercent >= electricCost)
                {
                    isCharging = false;
                    hasCharged = false;
                    dashEffect.SetActive(true);
                    captureDirection();
                    findDashSpeed(chargedTime, dashDuration);
                    isDashing = true;
                }

                if (!isDashing)
                {
                    isCharging = true;
                }
                else
                {
                    if (durationTracker > 0 && currentDashes < maxDashes && canMultiDash)
                    {
                        captureDirection();
                        currentDashes++;
                        durationTracker = dashDuration;
                        GetComponent<PlayerMovement>().isDashing = false;
                    }
                }
            }
        }
    }

    public void unPressKey()
    {
        Debug.Log("UnpressKey");
        if (CoolDownTracker <= 0)
        {
            if (canDash)
            {
                canDash = false;
                if (chargedDash)
                {
                    if (!hasCharged)
                    {
                        isCharging = false;
                        getChargePercent(chargedTime);
                        hasCharged = true;
                    }
                }
            }
        }
    }

    private void getChargePercent(float _pressedTime)
    {
        if (_pressedTime > maxChargedSeconds)
        {
            _pressedTime = maxChargedSeconds;
        }

        chargePercent = (_pressedTime * 100) / maxChargedSeconds;
    }

    public void resetCharge()
    {
        isCharging = false;
        canDash = false;
        hasCharged = false;
        chargedTime = 0;
        chargePercent = 0;

    }
}
