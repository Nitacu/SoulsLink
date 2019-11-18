using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private float wallDuration;

    private float _coolDownTracker;

    private PlayerAiming _aiming;

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
        _coolDownTracker = _coolDown;
    }

    private void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


    }
    public void stopShooting()
    {
        shooting = false;
    }

    public void pressKey()
    {
        if (_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            spawnWall();
        }
    }

    private bool shooting = false;
    public void startShoot()
    {
        shooting = true;
    }

    public void spawnWall()
    {
        //if (_coolDownTracker <= 0)
        //{
            Vector2 direction = GetComponent<PlayerAiming>().AimDirection;           
            GameObject wall = Instantiate(_wallPrefab, gameObject.transform.position, Quaternion.identity);
            StartCoroutine(destroyWall(wall, wallDuration));
            wall.transform.position = gameObject.transform.position;
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            wall.transform.rotation = Quaternion.Euler(0, 0, rot);
        //}
    }

    IEnumerator destroyWall(GameObject wall, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(wall);
    }
}
