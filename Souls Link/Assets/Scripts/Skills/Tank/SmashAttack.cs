using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashAttack : MonoBehaviour
{
    private GameObject chargeBar;
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    private float forceUsed = 0;
    private GameObject _attackReference;
    private bool isCharging = false;
    private float chargedTime = 0;
    private float _coolDownTracker = 0;
    public float _coolDown = 0;
    public float maxChargedSeconds = 3;
    private Vector2 attackDirection;
    public float attackDuration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

        chargeBarControl();

        if (isCharging)
        {
            chargedTime += Time.deltaTime;
        }
    }

    public void setSmashBar(GameObject smashBar)
    {
        chargeBar = smashBar;
    }


    private void findForce(float _pressedTime, float _dashDuration)
    {
        if (_pressedTime > maxChargedSeconds)
        {
            _pressedTime = maxChargedSeconds;
        }
        float pressedTimePercent = (_pressedTime * 100) / maxChargedSeconds;
        float distanceDifference = _maxForce - _minForce;
       

        forceUsed = ((pressedTimePercent * distanceDifference) / 100) + _minForce;
       
    }

    public void pressKey()
    {
        if (_coolDownTracker <= 0)
        {
            isCharging = true;
        }
    }

    public void unPressKey()
    {
        if (_coolDownTracker <= 0)
        {
            isCharging = false;
            captureDirection();
            _coolDownTracker = _coolDown;
            _attackReference = Instantiate(_attackPrefab, gameObject.transform);
            _attackReference.transform.localPosition = new Vector2(0, GetComponent<PlayerAiming>().getOffsetY());
            StartCoroutine(destroyAttack(attackDuration, _attackReference));
        }

    }

    IEnumerator destroyAttack(float time, GameObject attack)
    {
        yield return new WaitForSeconds(time);
        Destroy(attack);
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
        attackDirection = newDirection;
    }
}
