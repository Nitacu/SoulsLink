using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : Skill
{
    public float _coolDown = 0;
    public float _stealthTime = 0;
    public float _mistLifeTime = 0;
    public GameObject _mist;
    private float _coolDownTracker = 0;
    private float _stealthTimeTracker = 0;
    [Header("Offset Y of mist spawn")]
    public float offsetY = 0;

    private bool isStealth = false;
    private bool insideMist = false;
    public bool IsStealth { get => isStealth; set => isStealth = value; }
    public bool InsideMist { get => insideMist; set => insideMist = value; }

    // Start is called before the first frame update
    void Start()
    {
        _coolDownTracker = _coolDown;
        _stealthTimeTracker = _stealthTime;
    }

    // Update is called once per frame
    void Update()
    {
        checkStealth();

        if (_coolDownTracker <= _coolDown && _coolDownTracker > 0)
        {
            _coolDownTracker -= Time.deltaTime;
        }


        //Debug.Log("Estoy invisible?: " + isStealth);
    }

    public void pressKey()
    {
        spawnMist();
    }

    private void spawnMist()
    {
        if (_coolDownTracker <= 0)
        {
            _coolDownTracker = _coolDown;
            Vector3 newMistPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            GameObject temp = Instantiate(_mist, newMistPosition, Quaternion.identity);
            temp.GetComponent<MistAnimations>().setMist(GetComponent<Dash>().chargePercent, gameObject);

            StartCoroutine(destroyMist(temp, _mistLifeTime));
        }
    }

    IEnumerator destroyMist(GameObject mist, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(mist);
    }

    private void checkStealth()
    {
        if (!insideMist)
        {
            if (isStealth)
            {
                Color tmp = GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color;
                tmp.a = 0.5f;
                gameObject.GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color = tmp;

                gameObject.layer = LayerMask.NameToLayer("Invisible");

                if (_stealthTimeTracker <= _stealthTime && _stealthTime > 0)
                {
                    _stealthTimeTracker -= Time.deltaTime;
                }

                if (_stealthTimeTracker <= 0)
                {
                    isStealth = false;
                    _stealthTimeTracker = _stealthTime;
                }
            }
            else
            {
                Color tmp = GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                gameObject.GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color = tmp;
                gameObject.layer = LayerMask.NameToLayer("Player");
                disolveStealth();
            }
        }
        else
        {
            isStealth = true;
            Color tmp = GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.1f;
            gameObject.GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>().color = tmp;
            gameObject.layer = LayerMask.NameToLayer("Invisible");
        }
    }    

    public virtual void disolveStealth()
    {

    }

    public void activateInsideMist()
    {
        insideMist = true;
        _stealthTimeTracker = _stealthTime;
    }

    public void resetStealth()
    {
        insideMist = false;
        isStealth = false;
        _stealthTimeTracker = _stealthTime;
    }
}
