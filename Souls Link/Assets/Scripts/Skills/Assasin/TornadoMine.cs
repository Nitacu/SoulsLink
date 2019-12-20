using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMine : Skill
{

    public float _bombLifeTime = 0;
    public float _tornadoLifeTime = 0;
    public float _tornadoDamage = 0;
    public GameObject _bomb;
    public GameObject _tornado;
    [Header("Offset Y of bomb spawn")]
    public float offsetY = -0.3f;

  

    // Start is called before the first frame update
    void Start()
    {
        CoolDownTracker = _coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (CoolDownTracker <= _coolDown && CoolDownTracker > 0)
        {
            CoolDownTracker -= Time.deltaTime;
        }
        
    }

    public void spawnBomb()
    {
        if (CoolDownTracker <= 0)
        {
            CoolDownTracker = _coolDown;
            Vector3 newBombPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            GameObject temp = Instantiate(_bomb, newBombPosition, Quaternion.identity);
            temp.GetComponent<MineController>().setBomb(_tornadoLifeTime, _tornadoDamage, _tornado, GetComponent<Dash>().chargePercent, gameObject);
            StartCoroutine(destroyBomb(temp, _bombLifeTime));
        }
    }

    IEnumerator destroyBomb(GameObject bomb, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bomb);
    }
}
