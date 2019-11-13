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
    [SerializeField] private KeyCode _inputAttack;
    private BoxCollider2D _collider;
    [Header("Simple Dash Settings")]
    public float dashSpeed = 0;


    private AimCursor _aiming;
    public AimCursor Aiming { get => _aiming; set => _aiming = value; }
    private bool isDashing = false;

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

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _coolDownTracker = _coolDown;
        durationTracker = dashDuration;
        _aiming = GetComponent<AimCursor>();
        chargeBar = GameObject.FindGameObjectWithTag("chargeBar");
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

    }

    public void playerDash(Vector2 direction)
    {           
        _rb.velocity = dashDirection * dashSpeed;
        GetComponent<PlayerMove>().enabled = true;
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
        Vector2 newDirection = new Vector2(Input.GetAxis(GetComponent<PlayerMove>().AxisX), Input.GetAxis(GetComponent<PlayerMove>().AxisY)).normalized;
        if (newDirection == Vector2.zero)
        {
            newDirection = GetComponent<AimCursor>().LastVector;
        }
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
            
            if (Input.GetKey(_inputAttack))
            {
                chargedTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(_inputAttack))
            {
                _collider.isTrigger = true;
                captureDirection();
                findDashSpeed(chargedTime, dashDuration);
                isDashing = true;

            }
        }
        else
        {
            if (durationTracker <= 0)
            {
                isDashing = false;
                _collider.isTrigger = false;
                chargedTime = 0;
                currentDashes = 0;
                durationTracker = dashDuration;
                _coolDownTracker = _coolDown;
                GetComponent<PlayerMove>().IsDashing = false;
            }
            else
            {
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMove>().IsDashing = true;
                if (Input.GetKeyDown(_inputAttack) && currentDashes <= maxDashes && canMultiDash)
                {
                    captureDirection();
                    currentDashes++;
                    durationTracker = dashDuration;
                    GetComponent<PlayerMove>().IsDashing = false;
                }
            }
        }
    }

    private void dashCheck()
    {
        if (!isDashing)
        {
            if (Input.GetKeyDown(_inputAttack))
            {
                captureDirection();
                currentDashes = 0;
                isDashing = true;
                _collider.isTrigger = true;
            }
        }
        else
        {
            if (durationTracker <= 0)
            {
                isDashing = false;
                _collider.isTrigger = false;
                durationTracker = dashDuration;
                currentDashes = 0;
                _coolDownTracker = _coolDown;
                GetComponent<PlayerMove>().IsDashing = false;
            }
            else
            {
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMove>().IsDashing = true;
                if (Input.GetKeyDown(_inputAttack) && currentDashes < maxDashes && canMultiDash)
                {
                    currentDashes++;
                    durationTracker = dashDuration;
                    captureDirection();
                    GetComponent<PlayerMove>().IsDashing = false;
                }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
        }
    }
}
