using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    private Vector2 dashDirection;
    private GameObject chargeBar;
    private Rigidbody2D _rb;
    [Header("General Settings")]
    private float _coolDownTracker = 0;
    public float _coolDown = 0;
    public float dashDuration = 0;
    private float durationTracker = 0;
    public float _damage = 0;
    private CircleCollider2D _collider;
    public GameObject _dashCollider;
    [Header("Simple Dash Settings")]
    public float dashSpeed = 0;


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

    // Start is called before the first frame update
    void Start()
    {
        _dashCollider.SetActive(false);
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _coolDownTracker = _coolDown;
        durationTracker = dashDuration;
        _aiming = GetComponent<PlayerAiming>();
        
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
        }
    }

    public void playerDash(Vector2 direction)
    {           
        _rb.velocity = dashDirection * dashSpeed;
        GetComponent<PlayerMovement>().enabled = true;
        //Debug.Log(direction);
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
        dashDirection = newDirection;
    }

    private void chargeDash()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }
        if (_coolDownTracker <= 0)
        {
            chargedDashCheck();
        }
    }

    private void simpleDash()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }
        if (_coolDownTracker <= 0)
        {
            dashCheck();
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
                isDashing = false;
               
                chargedTime = 0;
                currentDashes = 0;
                durationTracker = dashDuration;
                _coolDownTracker = _coolDown;
                GetComponent<PlayerMovement>().isDashing = false;
            }
            else
            {
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMovement>().isDashing = true;
                /*if (Input.GetKeyDown(_inputAttack) && currentDashes <= maxDashes && canMultiDash)
                {
                    captureDirection();
                    currentDashes++;
                    durationTracker = dashDuration;
                    GetComponent<PlayerMove>().IsDashing = false;
                }*/
            }
        }
    }

    private void dashCheck()
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
                isDashing = false;
                
                durationTracker = dashDuration;
                currentDashes = 0;
                _coolDownTracker = _coolDown;
                GetComponent<PlayerMovement>().isDashing = false;
            }
            else
            {
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMovement>().isDashing = true;
                /*if (Input.GetKeyDown(_inputAttack) && currentDashes < maxDashes && canMultiDash)
                {
                    currentDashes++;
                    durationTracker = dashDuration;
                    captureDirection();
                    GetComponent<PlayerMove>().IsDashing = false;
                }*/
            }
        }
    }

    private void findDashSpeed(float _pressedTime, float _dashDuration)
    {
        if (_pressedTime > maxChargedSeconds)
        {
            _pressedTime = maxChargedSeconds;
            
        }


        float pressedTimePercent = (_pressedTime * 100) / maxChargedSeconds;
        float distanceDifference = maxDashSpeed - minDashSpeed;
        float durationDifference = maxDuration - minDuration;

        dashDuration = ((pressedTimePercent * durationDifference) / 100) + minDuration;
        durationTracker = dashDuration;
        dashSpeed = ((pressedTimePercent * distanceDifference) / 100) + minDashSpeed;
    }

   

    public void pressKey()
    {

        if (_coolDownTracker <= 0)
        {
            canDash = true;
            if (!chargedDash)
            {
                //simple
                if (!isDashing)
                {
                    captureDirection();
                    currentDashes = 0;
                    isDashing = true;
                   
                }
                else
                {
                    if (durationTracker > 0 && currentDashes < maxDashes && canMultiDash)
                    {
                        currentDashes++;
                        durationTracker = dashDuration;
                        captureDirection();
                        GetComponent<PlayerMovement>().isDashing = false;
                    }
                }
            }
            else
            {
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
        if (_coolDownTracker <= 0)
        {
            if (canDash)
            {
                canDash = false;
                if (chargedDash)
                {
                    if (!isDashing)
                    {
                        isCharging = false;                       
                        captureDirection();
                        findDashSpeed(chargedTime, dashDuration);
                        isDashing = true;

                    }
                }
            }
        }
    }
}
