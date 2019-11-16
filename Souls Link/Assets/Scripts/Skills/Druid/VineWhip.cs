using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWhip : MonoBehaviour
{
    [SerializeField] private GameObject _vineWhip;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _damage;
    [SerializeField] private float _rootDuration;
    private float _coolDownTracker;
    private Vector2 direction;
    [SerializeField] private KeyCode _inputAttack;

    private AimCursor _aiming;

    private float xOffset = 0;
    private float yOffset = -0.3f;

    private void Start()
    {
        _aiming = GetComponent<AimCursor>();
    }

    private void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


        if (Input.GetKeyDown(_inputAttack) && _coolDownTracker <= 0)
        {
            useRoot(_aiming.LastVector.normalized);

            _coolDownTracker = _coolDown;
        }
    }

    IEnumerator destroyRoot(GameObject root, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(root);
    }

    private void useRoot(Vector2 direction)
    {
        GameObject root = Instantiate(_vineWhip, gameObject.transform);

        root.GetComponentInChildren<VineSkillControl>().setVineSkill(direction, _damage);
        StartCoroutine(destroyRoot(root, _rootDuration));

        xOffset = 0;
        yOffset = -0.3f;

        determineOffset(direction);

        root.transform.localPosition = new Vector2(0 + xOffset, 0 + yOffset);

    }

    private void getDirection()
    {
        direction = new Vector2(Input.GetAxis(GetComponent<PlayerMove>().AxisX), Input.GetAxis(GetComponent<PlayerMove>().AxisY)).normalized;
        if (direction == Vector2.zero)
        {
            direction = GetComponent<AimCursor>().LastVector;
        }
    }

    private void determineOffset(Vector2 direction)
    {
        if (direction.x == 0 && direction.y > 0)
        {
            xOffset = 0.1f;
        }

        if (direction.x == 0 && direction.y < 0)
        {
            xOffset = -0.1f;
        }

        if (direction.y == 0 && direction.x > 0)
        {
            yOffset = -0.2f;
        }

        if (direction.y == 0 && direction.x < 0)
        {
            yOffset = 0f;
        }
    }
}
