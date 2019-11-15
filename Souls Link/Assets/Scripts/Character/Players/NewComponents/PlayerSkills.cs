using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

public class Skills
{
    public bool pressedDown;
    public bool pressed;
    public bool pressedUp;

    public bool GetDown()
    {
        return pressedDown;
    }

    public bool Get()
    {
        return pressed;
    }

    public bool GetUp()
    {
        return pressedUp;
    }
}

public class PlayerSkills : MonoBehaviour
{
    private PlayerInputActions _inputControl;

    public enum InputSkill
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4
    }

    private void Awake()
    {
        _inputControl = new PlayerInputActions();
    }

    private void Update()
    {
    }

    public delegate void DelegateSkill1();
    public DelegateSkill1 _skill1Down;

    [Header("Skill 1")]
    [SerializeField] UnityEvent Skill1PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill1PressUp = new UnityEvent();
    private void OnSkill1(InputValue value)
    {
        if (value.Get<float>() == 1)//Pressed
        {
            if (Skill1PressDown != null) Skill1PressDown.Invoke();

        }
        else if (value.Get<float>() == 0)//Released
        {
            if (Skill1PressUp != null) Skill1PressUp.Invoke();

        }
    }


    [Header("Skill 2")]
    [SerializeField] UnityEvent Skill2Pressed = new UnityEvent();
    private void OnSkill2()
    {
        if (Skill2Pressed != null) Skill2Pressed.Invoke();
    }

    [Header("Skill 3")]
    [SerializeField] UnityEvent Skill3Pressed = new UnityEvent();
    private void OnSkill3()
    {
        if (Skill3Pressed != null) Skill3Pressed.Invoke();
    }

    [Header("Skill 4")]
    [SerializeField] UnityEvent Skill4Pressed = new UnityEvent();
    private void OnSkill4()
    {
        if (Skill4Pressed != null) Skill4Pressed.Invoke();
    }



    //Enable and Disable
    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }
}
