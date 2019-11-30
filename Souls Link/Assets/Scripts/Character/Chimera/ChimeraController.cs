using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraController : MonoBehaviour
{
    [SerializeField] private float _speed = 100;
    public float Speed
    {
        get { return _speed; }
    }

    private List<GameObject> _players = new List<GameObject>();

    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private Vector2 _movement;
    [SerializeField] private Vector2[] _inputsMovements;

    bool[] _unFusionCheck;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void LateUpdate()
    {

        calculateNewMovement();
        move();
    }

    private void move()
    {
        _rb.velocity = _movement * Time.deltaTime * _speed;

        setAnimation();
    }

    private void setAnimation()
    {
        //RotarSprite
        if (_movement.x > 0)
        {
            _renderer.flipX = false;
        }
        else if (_movement.x < 0)
        {
            _renderer.flipX = true;
        }        
    }

    public void sendMovement(Vector2 movement, int id)
    {
        _inputsMovements[id] = movement;
    }

    private void calculateNewMovement()
    {
        Vector2 newinputMovement = Vector2.zero;
        foreach (Vector2 input in _inputsMovements)
        {
            newinputMovement += input;
        }

        _movement = newinputMovement;
    }


    public void setPlayersInFusion(List<GameObject> players)
    {
        _players = players;
        setPlayersChild();
    }

    private void setPlayersChild()
    {
        Debug.Log("Current Players: " + _players.Count);
        _inputsMovements = new Vector2[_players.Count];

        _unFusionCheck = new bool[_players.Count];
        resetChekingUnfusion();

        int idCount = 0;

        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().IsOnFusion = true;
            player.GetComponent<FusionTrigger>().CurrentChimeraParent = this;
            player.GetComponent<FusionTrigger>().OnFusionID = idCount;
            player.transform.SetParent(gameObject.transform);
            player.transform.localPosition = Vector3.zero;
            player.GetComponent<FusionTrigger>().assingSkillsTochimera(gameObject);

            //player.transform.position = gameObject.transform.position;

            idCount++;
            //player.SetActive(false);
        }
    }

    public void addNewPlayer(GameObject newPlayer)
    {
        _players.Add(newPlayer);

        _inputsMovements = new Vector2[_players.Count];
        _unFusionCheck = new bool[_players.Count];

        resetChekingUnfusion();

        newPlayer.GetComponent<FusionTrigger>().IsOnFusion = true;
        newPlayer.GetComponent<FusionTrigger>().CurrentChimeraParent = this;
        newPlayer.GetComponent<FusionTrigger>().OnFusionID = _players.Count;
        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.GetComponent<FusionTrigger>().assingSkillsTochimera(gameObject);

    }

    //UnFusion methods

    public void sendUnFusion(bool check, int id)
    {
        _unFusionCheck[id] = check;

        //Ver que todos se queiren desfusionar
        bool allPlayersWantUnfusion = true;
        foreach (bool checkUnfusion in _unFusionCheck)
        {
            if (!checkUnfusion)
            {
                allPlayersWantUnfusion = false;
            }
        }

        if (allPlayersWantUnfusion)
        {
            //desfusionar
            unFusion();
        }
    }

    private void unFusion()
    {
        Debug.Log("DesFusionar");

        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().IsOnFusion = false;
            player.GetComponent<FusionTrigger>().CurrentChimeraParent = null;
            //player.GetComponent<FusionTrigger>().OnFusionID = idCount;
            player.GetComponent<FusionTrigger>().ActiveComponentsOnFusion();

            player.transform.parent = null;
           
        }

        Destroy(gameObject);
    }

    private void resetChekingUnfusion()
    {
        for (int i = 0; i < _unFusionCheck.Length; i++)
        {
            _unFusionCheck[i] = false;
        }
    }

}
