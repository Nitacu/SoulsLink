using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControlFusion : MonoBehaviour
{
    [SerializeField] private GameObject _zord;

    //variable que se usan durante la creacion del zord
    private float _middlePositionX;
    private float _middlePositionY;
    private GameObject _zordAux;
    private List<Collider2D> _listPilots = new List<Collider2D>();

    public void createZord(Collider2D[] players)
    {
        if (allPlayersWillingToMerge(players))
        {

            _middlePositionX = 0;
            _middlePositionY = 0;
            _listPilots = players.ToList();

            // crea el zord 
            _zordAux = Instantiate(_zord);
            _zordAux.GetComponent<ZordMove>().enabled = false;
            // para cuadrar el zord en el punto medio
            while (_listPilots.Count > 0)
            {
                //para el calculo de la distancia media
                _middlePositionX += _listPilots[0].transform.position.x;
                _middlePositionY += _listPilots[0].transform.position.y;
                //agrega los axis de los player que se unieron
                _zordAux.GetComponent<ZordMove>().ListAxisX.Add(_listPilots[0].GetComponent<PlayerMove>().AxisX);
                _zordAux.GetComponent<ZordMove>().ListAxisY.Add(_listPilots[0].GetComponent<PlayerMove>().AxisY);
                //activa las particulas de escencia
                _listPilots[0].GetComponent<ControlPlayerFusion>().releaseEssence(_zordAux);
                //desactiva los players
                _listPilots[0].gameObject.SetActive(false);
                _listPilots.RemoveAt(0);
            }

            //ubica en la posicion al zord y al atractor de particulas
            _zordAux.transform.position = new Vector2(_middlePositionX / players.Length, _middlePositionY / players.Length);

            StartCoroutine(activeZord());
        }
    }

    IEnumerator activeZord()
    {
        yield return new WaitForSeconds(3);
        _zordAux.GetComponent<ZordMove>().enabled = true;
    }

    private bool allPlayersWillingToMerge(Collider2D[] players)
    {
        foreach (Collider2D collider in players)
        {
            if (!collider.GetComponent<ControlPlayerFusion>().AllowTransformation)
            {
                return false;
            }
        }

        return true;
    }

}