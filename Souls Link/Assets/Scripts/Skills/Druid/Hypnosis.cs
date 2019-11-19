using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnosis : MonoBehaviour
{

    [SerializeField] private GameObject _hypnosisPrefab;
    private GameObject _hypnosisReference;
    [SerializeField] private float _hypnosisFlashTime;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _hypnosisDuration;
    private float _coolDownTracker;
    private Vector2 direction;


    public bool stillAttackOption = false;

    // Start is called before the first frame update
    void Start()
    {
        _coolDownTracker = _coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }

    }

    private void stopMoving()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
    }

    private void backToNormal()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }

    public void hypnotize()
    {
        if (_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            stopMoving();

            if (stillAttackOption)
            {
                _hypnosisReference = Instantiate(_hypnosisPrefab, GetComponent<PlayerAiming>().getPosition(), Quaternion.identity);
            }
            else
            {
                _hypnosisReference = Instantiate(_hypnosisPrefab, gameObject.transform);
                _hypnosisReference.transform.localPosition = new Vector2(0, GetComponent<PlayerAiming>().getOffsetY());

            }
            
            getDirection();
            _hypnosisReference.GetComponentInChildren<HypnosisSkillControl>().setHypnosis(direction, _attackDamage);
            //hypnosisReference.GetComponentInChildren<StrongAttackController>().setDirection(direction);
            StartCoroutine(destroyHypnoFlash(_hypnosisReference, _hypnosisFlashTime));
        }
    }

    private void getDirection()
    {
        direction = GetComponent<PlayerAiming>().AimDirection.normalized;
    }

    IEnumerator destroyHypnoFlash(GameObject hypnosis, float time)
    {
        yield return new WaitForSeconds(time);
        backToNormal();
        Destroy(hypnosis);
    }
}
