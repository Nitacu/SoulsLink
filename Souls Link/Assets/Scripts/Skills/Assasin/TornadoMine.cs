using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMine : MonoBehaviour
{

    public float _coolDown = 0;
    public float _bombLifeTime = 0;
    public float _tornadoLifeTime = 0;
    public float _tornadoDamage = 0;
    [SerializeField] private KeyCode _inputAttack;
    public GameObject _bomb;
    public GameObject _tornado;
    private float _coolDownTracker = 0;
    [Header("Offset Y of bomb spawn")]
    public float offsetY = 0;

  

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

        if (Input.GetKeyDown(_inputAttack) && _coolDownTracker <= 0)
        {
            spawnBomb();
            _coolDownTracker = _coolDown;
        }
    }

    private void spawnBomb()
    {
        Vector3 newBombPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        GameObject temp = Instantiate(_bomb, newBombPosition, Quaternion.identity);
        temp.GetComponent<MineController>().setBomb(_tornadoDamage, _tornadoDamage, _tornado);
        StartCoroutine(destroyBomb(temp, _bombLifeTime));
    }

    IEnumerator destroyBomb(GameObject bomb, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bomb);
    }
}
