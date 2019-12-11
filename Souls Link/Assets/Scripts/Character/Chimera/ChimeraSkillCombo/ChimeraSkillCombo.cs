using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraSkillCombo", menuName = "ScriptableObjects/ChimeraSkillCombo", order = 1)]
public class ChimeraSkillCombo : ScriptableObject
{
    public PlayerSkills.SkillType[] _skill1;
}