using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private float _distanceFactor = 1;

    private PlayerMove _playerReference;

    private Vector2 _aimVector;
    private Vector2 _lastVector;
    public Vector2 AimVector { get => _aimVector;}
    public Vector2 LastVector { get => _lastVector; set => _lastVector = value; }

    
    enum DirectionType
    {
        CURSORAIM,
        MOVEMENTAIM
    }

    [SerializeField] private DirectionType _directionType;
    // Start is called before the first frame update
    void Start()
    {
        _playerReference = GetComponent<PlayerMove>();

        switch (_directionType)
        {
            case DirectionType.CURSORAIM:
                setcrossHairCursor();
                break;
            case DirectionType.MOVEMENTAIM:
                setCrossHairMovementDirection();
                break;            
        }        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_directionType)
        {
            case DirectionType.CURSORAIM:
                setcrossHairCursor();
                break;
            case DirectionType.MOVEMENTAIM:
                setCrossHairMovementDirection();
                break;
        }
    }

    public void setCrossHairMovementDirection()
    {
        Vector2 _direction = _playerReference.Movement;
        _aimVector = _direction.normalized;
        

        if (AimVector.magnitude > 0)
        {
            _aimVector *= _distanceFactor;
            _crossHair.transform.localPosition = _aimVector;
            _crossHair.SetActive(true);
        }
        else
        {
            _crossHair.SetActive(false);
        }
    }

    private void setcrossHairCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _aimVector = mousePosition - (Vector2)transform.position;
        _aimVector.Normalize();

        if (AimVector.magnitude > 0)
        {
            _aimVector *= _distanceFactor;
            _crossHair.transform.localPosition = _aimVector;
            _crossHair.SetActive(true);
        }
        else
        {
            _crossHair.SetActive(false);
        }

    }
}
