using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public PlayerSkills.SkillType _skillType;
    public float _coolDown;
    protected float coolDownTracker;
    protected bool isCasting;

    public float CoolDownTracker { get => coolDownTracker; set => coolDownTracker = value; }
    public bool IsCasting { get => isCasting; set => isCasting = value; }
}
