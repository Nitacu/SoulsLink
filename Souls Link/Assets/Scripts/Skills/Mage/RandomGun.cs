using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGun : Skill
{
    [SerializeField] private GameObject _energyBalls;
    [SerializeField] private GameObject _turret;
    [SerializeField] private GameObject _randomGunPickerPosition;
    [SerializeField] private float _damage;

    private PlayerAiming _aiming;

    private bool pickedWeapon = false;
    private bool hasWeapon = false;
    public bool HasWeapon { get => hasWeapon; set => hasWeapon = value; }
    private GameObject currentGun;


    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        CoolDownTracker = _coolDown;
        CoolDownTracker = 0;
    }

    private void Update()
    {

        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
    }

    public void pressKey()
    {
        if (CoolDownTracker <= 0)
        {
            chooseWeapon();
            CoolDownTracker = _coolDown;
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

    public void canPickAgain()
    {
        pickedWeapon = false;
    }

    public void chooseWeapon()
    {
        //if (_coolDownTracker <= 0)
        //{
        

        Vector2 direction = _aiming.AimDirection;
        CoolDownTracker = _coolDown;
        Vector2 newPos = Vector2.zero;
        newPos.y = newPos.y - 0.2f;

        if (!pickedWeapon)
        {
            pickedWeapon = true;
            _randomGunPickerPosition.GetComponent<RandomGunChooser>().startChoosing();
        }
        
        

        
        //}
    }

    public void spawnEnergyBalls()
    {
        if (!hasWeapon)
        {
            currentGun = Instantiate(_energyBalls, gameObject.transform);
            hasWeapon = true;
        }
        else
        {
            Destroy(currentGun);
            currentGun = Instantiate(_energyBalls, gameObject.transform);
            hasWeapon = true;
        }
        Invoke("canPickAgain", _coolDown);
    }

    public void spawnTurret()
    {
        if (!hasWeapon)
        {
            currentGun = Instantiate(_turret, gameObject.transform.position, Quaternion.identity);
            hasWeapon = true;
        }
        else
        {
            Destroy(currentGun);
            currentGun = Instantiate(_turret, gameObject.transform.position, Quaternion.identity);
            hasWeapon = true;
        }
        Invoke("canPickAgain", _coolDown);
    }





}
