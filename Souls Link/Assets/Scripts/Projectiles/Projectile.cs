using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _damage;

    public enum ProjectileOwner
    {
        PLAYER,
        ENEMY
    }

    public ProjectileOwner _projetileOwner;

    protected bool _reflected;
    public bool Reflected
    {
        get { return _reflected; }
        set { _reflected = value; }
    }

    public virtual void reflectMySelf()
    {

    }
}
