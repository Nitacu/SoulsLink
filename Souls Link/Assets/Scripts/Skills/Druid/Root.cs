using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Skill
{
    [SerializeField] private GameObject _rootPrefab;
    [SerializeField] private GameObject _rootEffectPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _damage;
    [SerializeField] private float _rootEffectDuration;
    [SerializeField] private float _rootDuration;
    [SerializeField] private float _rootSpawnRate;
    private float _coolDownTracker;
    private bool keepSpawning = false;
    private float cont = 0;
    private float offsetRoots = 1.08f;

    private PlayerAiming _aiming;

    private float xOffset = 0;
    private float yOffset = -0.3f;

    public bool KeepSpawning { get => keepSpawning; set => keepSpawning = value; }
    public float Cont { get => cont; set => cont = value; }

    private void Start()
    {
        _aiming = GetComponent<PlayerAiming>();
    }

    private void Update()
    {
        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


    }

    public void pressKey()
    {
        if(_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            KeepSpawning = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            useRoot(_aiming.AimDirection);
            GetComponentInChildren<Animator>().SetBool("isCasting", true);
            GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void unPressKey()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponentInChildren<Animator>().SetBool("isCasting", false);
        KeepSpawning = false;
    }

    IEnumerator destroyRoot(GameObject root, float time)
    {
        yield return new WaitForSeconds(time);
        root.GetComponent<RootSkillController>().destroyAnimation();
    }

    IEnumerator spawnRoot(Vector2 _direction, float _spawnRate)
    {
        yield return new WaitForSeconds(_spawnRate);
        useRoot(_direction);
    }

    private void useRoot(Vector2 direction)
    {
        GameObject root = Instantiate(_rootPrefab, gameObject.transform.position, Quaternion.identity);
        root.GetComponent<RootSkillController>().setRoot(_rootEffectPrefab, _damage, _rootEffectDuration, gameObject, direction, _rootPrefab, _rootDuration, _rootSpawnRate);
        
        
    }

    private void determineOffset(Vector2 direction)
    {
        if (direction.x != 0)
        {
            xOffset = 0.5f * Mathf.Sign(direction.x);
        }
      
        if (direction.y != 0)
        {
            yOffset = 0.5f * Mathf.Sign(direction.y) - 0.3f;
        }
    }
}
