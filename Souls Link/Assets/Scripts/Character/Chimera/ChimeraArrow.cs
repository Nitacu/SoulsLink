using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraArrow : MonoBehaviour
{
    [SerializeField] private GameManager.Characters _characterType;
    public GameManager.Characters CharacterType
    {
        get { return _characterType; }
    }

}
