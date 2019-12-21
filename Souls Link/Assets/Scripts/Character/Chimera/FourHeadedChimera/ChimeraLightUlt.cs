using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraLightUlt : Skill
{

    public float _coolDown = 0;  
    public float _tornadoDamage = 0;
    public GameObject _bomb;
    private float _coolDownTracker = 0;

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

    public void spawnBomb()
    {
        if (_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            Vector3 newBombPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject temp = Instantiate(_bomb, newBombPosition, Quaternion.identity);
           
        }
    }

    IEnumerator destroyBomb(GameObject bomb, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bomb);
    }
}
