using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public PlayerSkills.SkillType _skillType;
    public float _coolDown;
    protected float _coolDownTracker;
}
