using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAttack : MonoBehaviour
{

    [SerializeField] private KeyCode _inputSkill;
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

    private AimCursor _aiming;

    private void Start()
    {
        _aiming = gameObject.GetComponent<AimCursor>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_inputSkill) && !reflecting && _coolDownTracker <= 0)
        {
            reflecting = true;
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
        Debug.Log("Direction: " + _aiming.AimVector * 0.1f);
        Vector2 offset = (Vector2)transform.position + _aiming.AimVector * 0.5f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(offset, 0.3f, _aiming.AimVector, _coneDetectioneDistance);

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
