using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReadRotationFromParent : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private bool positive = true;
    [SerializeField] private float _zOffset;

    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Debug.Log("INICIO UPDATE startRotation: " + particle.startRotation);


        float zParentRotation = _parent.transform.eulerAngles.z;
        Debug.Log("zParentRotation: " + zParentRotation);

        zParentRotation *= (positive) ? 1 : -1;
        float newRotParticle = zParentRotation + _zOffset;
        Debug.Log("newRotParticle: " + newRotParticle);

        particle.startRotation = newRotParticle * Mathf.Deg2Rad;
        Debug.Log("FINAL UPDATE startRotation: " + particle.startRotation);
    }
}
