using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSkillController : MonoBehaviour
{
    private GameObject _rootEffect;
    private float _damage;
    private float _rootEffectDuration;
    private bool _enemyWasWalking = false;
    private GameObject playerReference;
    private Vector2 _directionRoot;
    private GameObject _newRootPrefab;
    private float _newRootDuration;
    private float _newSpawnRate;
    private bool hitWall = false;

    private const string ANIMATION_CLIP_BACKWARDS = "BackwardsRootSkill"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {                       
            if (collision.gameObject.GetComponent<SimpleEnemyController>().canWalk)
            {
                collision.gameObject.GetComponent<SimpleEnemyController>().stopWalking();
                _enemyWasWalking = true;
            }
            else
            {
                _enemyWasWalking = false;
            }
            
            collision.gameObject.GetComponent<SimpleEnemyController>().recieveDamage(_damage);
            GameObject temp = Instantiate(_rootEffect, collision.gameObject.transform.position, Quaternion.identity);
            temp.GetComponent<RootEffectControl>().setEffect(collision.gameObject, _rootEffectDuration, _enemyWasWalking);
        }

        if (collision.CompareTag("Wall"))
        {
           
        }
    }

    public void destroyAnimation()
    {
        GetComponent<Animator>().Play(Animator.StringToHash(ANIMATION_CLIP_BACKWARDS), -1, 0f);
    }

    public void setRoot(GameObject rootEffect, float damage, float rootEffectDuration, GameObject _player, Vector2 direction, GameObject _rootPrefab, float _rootDuration, float _spawnRate)
    {
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, rot);

        playerReference = _player;
        _rootEffect = rootEffect;
        _damage = damage;
        _rootEffectDuration = rootEffectDuration;
        _directionRoot = direction;
        _newRootPrefab = _rootPrefab;
        _newRootDuration = _rootDuration;
        _newSpawnRate = _spawnRate;

        StartCoroutine(destroyRoot(gameObject, _newRootDuration));

        //Raycast
        RaycastHit2D[] ray = Physics2D.RaycastAll(gameObject.transform.position, direction, 1.08f);

        Debug.DrawRay(gameObject.transform.position, direction * 1.08f, Color.red);

        foreach (RaycastHit2D _ray in ray)
        {
            if (_ray.collider.gameObject.CompareTag("Wall"))
            {
                hitWall = true;
                break;
            }
        }

        //If Ray
        //If no hits wall
        if (!hitWall)
        {
            StartCoroutine(spawnRoot(_newSpawnRate));
        }

    }

    IEnumerator spawnRoot(float time)
    {
        yield return new WaitForSeconds(time);
        spawnNextRoot();
    }

    private void spawnNextRoot()
    {
        Vector2 newPos = (Vector2)gameObject.transform.position + (_directionRoot * 1.08f);      
        GameObject root = Instantiate(_newRootPrefab, newPos, Quaternion.identity);
        root.GetComponent<RootSkillController>().setRoot(_rootEffect, _damage, _rootEffectDuration, gameObject, _directionRoot, _newRootPrefab, _newRootDuration, _newSpawnRate);
    }

    public void destroyMyself()
    {
        Destroy(gameObject);
    }

    IEnumerator destroyRoot(GameObject root, float time)
    {
        yield return new WaitForSeconds(time);
        destroyAnimation();
    }
}
