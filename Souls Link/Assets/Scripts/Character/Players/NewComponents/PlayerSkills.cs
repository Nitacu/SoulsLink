﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

public class PlayerSkills : MonoBehaviour
{
    const float DELAY = 0.05f;

    private PlayerInputActions _inputControl;
    private FusionTrigger _fusionTriggerRef;

    #region delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;

    public delegate void DelegateMultiplayerControllerSendSkill(float value, float index);
    public DelegateMultiplayerControllerSendSkill _pushValueSkill;
    #endregion

    public enum SkillType
    {
        //MAGE
        ICE_STAKE,
        REFFLECT,
        DECOY,
        RANDOM_GUN,
        //DRUID
        ROOT,
        VINE_WHIP,
        WALL_SKILL,
        POSION_DART,
        //ASSASSIN
        STRONG_ATTACK,
        TORNADO_MINE,
        DASH,
        MIST,
        //TANK
        FLAMETHROWER,
        HOOK,
        MAGMA_RING,
        SMASH_ATTACK
    }

    private void Awake()
    {
        _inputControl = new PlayerInputActions();        
    }
    

    IEnumerator skillDelay(float value, UnityEvent _eventDown, UnityEvent _eventUp, float index)
    {
        System.Type type = _eventDown.GetPersistentTarget(0).GetType();
        Skill _skillType = gameObject.GetComponent(type) as Skill;



        if (IsOnFusion())
        {
           // _fusionTriggerRef.CurrentChimeraParent.GetComponent<ChimeraSkillsController>().sendSkill(_skillType._skillType, value);
        }

        if (_isMine())
        {
            _pushValueSkill(value, index);

            yield return new WaitForSeconds(DELAY);

            if (value == 1)//Pressed
            {
                if (_eventDown != null) _eventDown.Invoke();
            }
            else if (value == 0)//Released
            {
                if (_eventUp != null) _eventUp.Invoke();
            }
        }
    }

    [Header("Skill 1")]
    [SerializeField] UnityEvent Skill1PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill1PressUp = new UnityEvent();
    private void OnSkill1(InputValue value)
    {
        StartCoroutine(skillDelay(value.Get<float>(), Skill1PressDown, Skill1PressUp, 1));
    }

    [Header("Skill 2")]
    [SerializeField] UnityEvent Skill2PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill2PressUp = new UnityEvent();
    private void OnSkill2(InputValue value)
    {
        StartCoroutine(skillDelay(value.Get<float>(), Skill2PressDown, Skill2PressUp, 2));
    }

    [Header("Skill 3")]
    [SerializeField] UnityEvent Skill3PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill3PressUp = new UnityEvent();
    private void OnSkill3(InputValue value)
    {
        StartCoroutine(skillDelay(value.Get<float>(), Skill3PressDown, Skill3PressUp, 3));
    }

    [Header("Skill 4")]
    [SerializeField] UnityEvent Skill4PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill4PressUp = new UnityEvent();

    private void OnSkill4(InputValue value)
    {
        StartCoroutine(skillDelay(value.Get<float>(), Skill4PressDown, Skill4PressUp, 4));
    }

    [Header("Dash")]
    [SerializeField] UnityEvent dashPressDown = new UnityEvent();

    private void OnDash(InputValue value)
    {
        StartCoroutine(skillDelay(value.Get<float>(), dashPressDown, null, 5));
    }

    //Enable and Disable
    private void OnEnable()
    {
        _inputControl.Enable();
        _fusionTriggerRef = GetComponent<FusionTrigger>();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }

    ////////////////////////// GET Y SET /////////////////////////
    public UnityEvent Skill1PressDown1 { get => Skill1PressDown; set => Skill1PressDown = value; }
    public UnityEvent Skill1PressUp1 { get => Skill1PressUp; set => Skill1PressUp = value; }
    public UnityEvent Skill4PressUp1 { get => Skill4PressUp; set => Skill4PressUp = value; }
    public UnityEvent Skill4PressDown1 { get => Skill4PressDown; set => Skill4PressDown = value; }
    public UnityEvent Skill3PressDown1 { get => Skill3PressDown; set => Skill3PressDown = value; }
    public UnityEvent Skill3PressUp1 { get => Skill3PressUp; set => Skill3PressUp = value; }
    public UnityEvent Skill2PressDown1 { get => Skill2PressDown; set => Skill2PressDown = value; }
    public UnityEvent Skill2PressUp1 { get => Skill2PressUp; set => Skill2PressUp = value; }
    public UnityEvent DashPressDown { get => dashPressDown; set => dashPressDown = value; }

    public bool IsOnFusion()
    {
        bool sendMovementToChimera = false;
        if (_fusionTriggerRef != null)
        {
            if (_fusionTriggerRef.IsOnFusion)
            {
                sendMovementToChimera = true;
            }
        }

        return sendMovementToChimera;
    }

}
