using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Resurrection : MonoBehaviour
{
    //Components
    [Header("components")]
    [SerializeField] List<GameObject> _componentsToDeactivate;
    [SerializeField] SpriteRenderer _render;

    //State Vars
    private bool bleedingOut = false;

    //Scan Vars
    [Header("Scan Players")]
    [SerializeField] private float _scanPlayersRadius = 1f;
    [SerializeField] private LayerMask _playerMask;
    private GameObject _playerToRess = null;

    //Resurrecting Vars
    [Header("Resurrecting")]
    [SerializeField] private float _lifePerSecond = 50f;
    private bool _resurrectingPlayer = false;
    public float _totalHealing = 0;

    //HUD FEEDBACK
    [Header("HUD FEEDBACK")]
    [SerializeField] private GameObject _pressToRessPrefab;
    private GameObject _pressToRessObject;
    [SerializeField] private GameObject _healingBarPrefab;
    private GameObject _healingBar;
    private FillerBar _barFill;

    private void Update()
    {
        scanBleedingPlayers();

        if (_resurrectingPlayer)
        {
            healingPlayerToRess();
        }
    }

    #region GENERAL RESS METHODS
    //KNOW IF PLAYERS AROUND TO RESS
    private void scanBleedingPlayers()
    {
        if (!bleedingOut)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position, _scanPlayersRadius, _playerMask);

            if (hits.Length <= 1)
            {
                if (hits[0].gameObject == gameObject)
                {
                    Debug.Log("Jugador no encontrado. Soy (" + gameObject.name + ")");

                    _playerToRess = null;
                    if (_pressToRessObject != null)
                        Destroy(_pressToRessObject);
                    return;
                }
            }

            foreach (var hit in hits)
            {
                if (hit.gameObject != gameObject)
                {
                    Debug.Log("Jugador encontrado. Soy (" + gameObject.name + ") - Encontré (" + hit.gameObject + ")");

                    if (hit.gameObject.GetComponent<Resurrection>())//tiene el componente
                    {
                        Resurrection ressComp = hit.gameObject.GetComponent<Resurrection>();

                        if (ressComp.bleedingOut)//si ese jugador con el que hice hit esta herido
                        {
                            Debug.Log("Jugador encontrado esta herido");

                            if (_playerToRess != null)
                            {
                                if (hit.gameObject != _playerToRess)//si ya tengo un player al cual dal ress y encuentor uno diferente
                                {
                                    _playerToRess = hit.gameObject;
                                    _pressToRessObject = Instantiate(_pressToRessPrefab, gameObject.transform);
                                    _pressToRessObject.transform.localPosition = Vector3.zero;
                                }
                            }
                            else//si no tengo simplemente asignar
                            {
                                _playerToRess = hit.gameObject;
                                _pressToRessObject = Instantiate(_pressToRessPrefab, gameObject.transform);
                                _pressToRessObject.transform.localPosition = Vector3.zero;
                            }

                            return;
                        }
                    }
                }
            }
        }
    }

    //SET AS BLEEDING OR ALIVE
    public void setResurrection(bool alive)
    {
        Debug.Log("Set Bleeding: " + !alive);


        bleedingOut = !alive;

        //Animacion de herido
        if (alive)
        {
            _render.color = Color.white;
        }
        else
        {
            Color bleedingColor = new Color(1, 1, 1, 0.4f);
            _render.color = bleedingColor;
        }

        //Activar Skills
        PlayerSkills playerSkills = GetComponent<PlayerSkills>();
        playerSkills.enabled = alive;

        Skill[] skills = GetComponents<Skill>();
        foreach (var item in skills)
        {
            item.enabled = alive;
        }

        //Activar componentes
        foreach (var component in _componentsToDeactivate)
        {
            component.SetActive(alive);
        }


        if (alive)
        {
            Rigidbody2D rb;
            //añadirm rigidbody
            if (GetComponent<Rigidbody2D>())
            {
                rb = GetComponent<Rigidbody2D>();
            }
            else
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 0;
            rb.freezeRotation = true;
            GetComponent<PlayerMovement>().RigidBodyPlayer = rb;
        }
        else
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }

        //gameObject.GetComponent<CircleCollider2D>().enabled = alive;
        gameObject.GetComponent<PlayerHPControl>().enabled = alive;
        gameObject.GetComponent<PlayerMovement>().enabled = alive;
        gameObject.GetComponent<PlayerAiming>().enabled = alive;
    }

    //RESS BAR HANDLE
    public void setResurrectingBarByLifeHealing(float currentLifeHealing, float maxHealth)
    {
        float fillAmount = currentLifeHealing / maxHealth;

        _barFill.setFiller(fillAmount);
    }
    #endregion

    #region GIVING RESS
    public void ressPlayer()
    {
        PlayerHPControl playerHp = _playerToRess.GetComponent<PlayerHPControl>();
        _totalHealing = Mathf.Clamp(_totalHealing, 0, playerHp.MaxPlayerHealth);
        _totalHealing = Mathf.Ceil(_totalHealing);

        _playerToRess.GetComponent<Resurrection>().receiveRess(_totalHealing);

        Destroy(_healingBar);
    }

    private void healingPlayerToRess()
    {
        PlayerHPControl playerHp = _playerToRess.GetComponent<PlayerHPControl>();
        float maxHealth = playerHp.MaxPlayerHealth;

        float healingFactor = Time.deltaTime * (_lifePerSecond);

        _totalHealing += healingFactor;
        _totalHealing = Mathf.Clamp(_totalHealing, 0, maxHealth);

        setResurrectingBarByLifeHealing(_totalHealing, maxHealth);
    }
    #endregion

    #region RECEIVING RESS
    public void receiveHealingToRess()
    {

    }

    public void receiveRess(float healingAmount)
    {
        GetComponent<PlayerHPControl>().healHP(healingAmount);
        GetComponent<Resurrection>().setResurrection(true);        
    }
    #endregion

    //INPUT
    public void OnRess(InputValue action)
    {
        if (!bleedingOut && _playerToRess != null)
        {
            float value = action.Get<float>();

            if (value == 1)//Pressed
            {
                _resurrectingPlayer = true;
                Debug.Log("Total healing en 0");
                _totalHealing = 0;

                //Crear Barra de revivir
                _healingBar = Instantiate(_healingBarPrefab, _playerToRess.transform);
                _barFill = _healingBar.GetComponent<FillerBar>();
            }
            else if (value == 0 && _resurrectingPlayer)//Unpressed
            {
                _resurrectingPlayer = false;

                ressPlayer();
            }
        }
        else
        {
            Debug.Log("No Players To Ress");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(gameObject.transform.position, _scanPlayersRadius);
    }
}
