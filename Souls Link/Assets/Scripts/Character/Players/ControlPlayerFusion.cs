using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aqui estaran las cosas para la funsion o invocacion del zord
public class ControlPlayerFusion : MonoBehaviour
{
    private bool _allowTransformation = false; // verifica que este objeto esta deacuerdo con fusionarse
    [SerializeField] private GameObject _essenceParticles; //particulas que aparecen cuando va a salir el zord
    [SerializeField] private KeyCode _keyAction; // boton para activar la fusion
    [SerializeField] private LayerMask _layerPlayer;

    public bool AllowTransformation { get => _allowTransformation; set => _allowTransformation = value; }

    private void Update()
    {
        if (Input.GetKey(_keyAction))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3.5f, _layerPlayer);
            AllowTransformation = true;
            if (colliders.Length > 1)
            {
                FindObjectOfType<ControlFusion>().createZord(colliders);
            }
        }

        if (Input.GetKeyUp(_keyAction))
        {
            AllowTransformation = false;
        }
    }

    public void releaseEssence(GameObject attactor)
    {
        Instantiate(_essenceParticles, transform.position, Quaternion.identity)
            .GetComponent<particleAttractorSpherical>().target = attactor.transform;
        
    }
}
