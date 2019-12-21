using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSucker : Skill
{
    public float _damage = 100;
    public float _lifeSuck = 100;
    public GameObject _bloodSuckerPrefab;
    private float _coolDownTracker = 0;
    [HideInInspector]
    public bool hasDealedDamage = false;
    private List<GameObject> _enemiesHit = new List<GameObject>();

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

    public void spawnBlood()
    {
        if (_coolDownTracker <= 0)
        {
            _enemiesHit.Clear();
            Vector2 direction = GetComponent<PlayerAiming>().AimDirection;
            _coolDownTracker = _coolDown;
            GameObject temp = Instantiate(_bloodSuckerPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<BloodSuckerController>().setBloodSucker(gameObject);
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            temp.transform.rotation = Quaternion.Euler(0, 0, rot - 90);
        }
    }

    public void dealDamage(GameObject _enemy)
    {       
        foreach(GameObject enemy in _enemiesHit)
        {
            if(enemy == _enemy)
            {
                hasDealedDamage = true;
            }
        }

        if (!hasDealedDamage)
        {
            _enemy.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            GetComponent<PlayerHPControl>().healHP(_lifeSuck);
            _enemiesHit.Add(_enemy);
        }
        else
        {
            hasDealedDamage = false;
        }
    }

    private void stopMoving()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<Animator>().SetBool("isCasting", true);
    }

    public void backToNormal()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
    }
}
