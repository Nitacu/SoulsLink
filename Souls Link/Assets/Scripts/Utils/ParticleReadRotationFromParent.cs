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
        float zParentRotation = _parent.transform.eulerAngles.z;

        zParentRotation *= (positive) ? 1 : -1;
        float newRotParticle = zParentRotation + _zOffset;

        particle.startRotation = newRotParticle * Mathf.Deg2Rad;
    }
}
