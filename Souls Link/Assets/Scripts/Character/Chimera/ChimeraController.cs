using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraController : MonoBehaviour
{
    [SerializeField] private float _speed = 100;

    private List<GameObject> _players = new List<GameObject>();

    private Rigidbody2D _rb;

    [SerializeField] private Vector2 _movement;
    [SerializeField] private Vector2[] _inputsMovements;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void LateUpdate()
    {
        Debug.Log("Calculate Movements");
        calculateNewMovement();
        move();
    }

    private void move()
    {
        _rb.velocity = _movement * Time.deltaTime * _speed;
        Debug.Log("Move");
    }

    public void sendMovement(Vector2 movement, int id)
    {
        _inputsMovements[id] = movement;   
    }

    private void calculateNewMovement()
    {     
        Debug.Log("Sumar inputs - count: " + _inputsMovements.Length);
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

        int idCount = 0;

        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().IsOnFusion= true;
            player.GetComponent<PlayerMovement>().CurrentChimeraParent = this;
            player.GetComponent<FusionTrigger>().OnFusionID = idCount;
            player.transform.SetParent(gameObject.transform);
            //player.transform.position = gameObject.transform.position;

            idCount++;
            //player.SetActive(false);
        }
    }
}
