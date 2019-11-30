using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometDash : MonoBehaviour
{
    private Vector2 dashDirection;
    private Rigidbody2D _rb;
    [Header("General Settings")]
    private float _coolDownTracker = 0;
    public float _coolDown = 0;
    public float dashDuration = 0;
    private float durationTracker = 0;
    public float _stunDuration = 0;
    private CircleCollider2D _collider;
    [Header("Simple Dash Settings")]
    public float dashSpeed = 0;


    private PlayerAiming _aiming;
    public PlayerAiming Aiming { get => _aiming; set => _aiming = value; }
    public bool isDashing = false;



    private bool canDash = false;

    // Start is called before the first frame update
    void Start()
    {
       
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _coolDownTracker = 0;
        durationTracker = dashDuration;
        _aiming = GetComponent<PlayerAiming>();

    }

    // Update is called once per frame
    void Update()
    {
        simpleDash();
    }

    public void playerDash(Vector2 direction)
    {
        //Saber si se debe enviar a chimera o moverme
        bool sendMovementToChimera = false;

        if (GetComponent<FusionTrigger>())
        {
            if (GetComponent<FusionTrigger>().IsOnFusion)
            {
                sendMovementToChimera = true;
            }
        }

        if (sendMovementToChimera)//enviar dash a chimera
        {
            ChimeraController chiControl = GetComponent<PlayerMovement>().FusionTriggerRef.CurrentChimeraParent;

            if (chiControl != null)
            {
                Debug.Log("Send Dash");
                float chimeraDashSpeed = dashSpeed;
                chiControl.sendMovement(dashDirection * (chimeraDashSpeed/(2)), GetComponent<PlayerMovement>().FusionTriggerRef.OnFusionID);
            }
        }
        else
        {
            GetComponent<PlayerMovement>().RigidBodyPlayer.velocity = dashDirection * dashSpeed;
        }
       
        GetComponent<PlayerMovement>().enabled = true;
        //Debug.Log(direction);
    }



    private void captureDirection()
    {
        Vector2 newDirection = GetComponent<PlayerAiming>().AimDirection;
        dashDirection = newDirection;
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


    private void dashCheck()
    {
        if (!isDashing)
        {
            
        }
        else
        {
           
            if (durationTracker <= 0)
            {
                isDashing = false;

                durationTracker = dashDuration;

                _coolDownTracker = _coolDown;
                GetComponent<PlayerMovement>().isDashing = false;
            }
            else
            {
                durationTracker -= Time.deltaTime;
                GetComponent<PlayerMovement>().isDashing = true;
                
            }
        }
    }


    public void pressKey()
    {

        if (_coolDownTracker <= 0)
        {
            canDash = true;

            //simple
            if (!isDashing)
            {
                captureDirection();

                isDashing = true;

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

            }
        }
    }

    public void resetDash()
    {
        isDashing = false;

        durationTracker = dashDuration;

        _coolDownTracker = _coolDown;
        GetComponent<PlayerMovement>().isDashing = false;
    }
}
