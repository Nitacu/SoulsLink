using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAttack : MonoBehaviour
{

    [SerializeField] private float _radiusDetection;
    [SerializeField] private float _coneDetectioneDistance;

    [SerializeField] private GameObject _shielAroundPrefab;
    [SerializeField] private GameObject _shieldDirectionPrefab;
    private GameObject _shieldReference;

    [SerializeField] private float _reflectingTime;
    private float _reflectingTimeTracker = 0;
    private bool reflecting;


    [SerializeField] private float _coolDown;
    private float _coolDownTracker;

    enum ReflectType
    {
        AROUND,
        DIRECTION
    }

    [SerializeField] ReflectType _reflectType;

    private PlayerAiming _aiming;
    private bool buttonPressed = false;

    private void Start()
    {
        _aiming = gameObject.GetComponent<PlayerAiming>();
    }

    public void pressKey()
    {
        buttonPressed = true;
    }

    private void stopMoving()
    {
        GetComponent<PlayerHPControl>().setReflectiveMode();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
        GetComponent<StakeAttack>().canShoot = false;
    }

    private void backToNormal()
    {
        GetComponent<PlayerHPControl>().setNormalMode();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
        GetComponent<StakeAttack>().canShoot = true;
    }

    private void Update()
    {
        if (buttonPressed && !reflecting && _coolDownTracker <= 0)
        {
            reflecting = true;
            stopMoving();
            _reflectingTimeTracker = _reflectingTime;

            //feedback start
            switch (_reflectType)
            {
                case ReflectType.AROUND:
                    _shieldReference = Instantiate(_shielAroundPrefab, gameObject.transform);
                    break;
                case ReflectType.DIRECTION:
                    _shieldReference = Instantiate(_shieldDirectionPrefab, gameObject.transform);
                    break;
            }
            _coolDownTracker = _coolDown;
        }

        if (reflecting)
        {
            if (_reflectingTimeTracker > 0)
            {
                _reflectingTimeTracker -= Time.deltaTime;

                switch (_reflectType)
                {
                    case ReflectType.AROUND:
                        detectProjectilesAround();
                        break;
                    case ReflectType.DIRECTION:
                        //detectoProjectilesOnDirection();
                        break;
                }


            }
            else
            {
                reflecting = false;
                buttonPressed = false;
                backToNormal();
                //feedback end
                Destroy(_shieldReference);
            }

        }

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }
    }

    private void detectoProjectilesOnDirection()
    {
        Debug.Log("Direction: " + _aiming.AimDirection * 0.1f);
        Vector2 offset = (Vector2)transform.position + _aiming.AimDirection * 0.5f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(offset, 0.3f, _aiming.AimDirection, _coneDetectioneDistance);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                GameObject other = hit.collider.gameObject;
                if (other.GetComponent<Projectile>() != null)
                {
                    Debug.Log("Projectile enter");
                    Projectile projectile = other.GetComponent<Projectile>();

                    if (!projectile.Reflected)
                    {
                        projectile.reflectMySelf();
                    }
                }
            }
        }
    }

    private void detectProjectilesAround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radiusDetection);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                GameObject other = hit.gameObject;
                if (other.GetComponent<Projectile>() != null)
                {
                    Debug.Log("Projectile enter");
                    Projectile projectile = other.GetComponent<Projectile>();

                    if (!projectile.Reflected)
                    {
                        projectile.reflectMySelf();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Color newColor = Color.blue;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, _radiusDetection);
    }

}
