using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private float _distanceFactor = 1;

    private GameObject _playerReference;

    private Vector2 _aimVector;

    public Vector2 AimVector { get => _aimVector;}


    // Start is called before the first frame update
    void Start()
    {
        setcrossHairCursor();
    }

    // Update is called once per frame
    void Update()
    {
        setcrossHairCursor();
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
