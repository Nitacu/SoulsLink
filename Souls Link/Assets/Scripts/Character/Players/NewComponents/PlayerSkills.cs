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
    [SerializeField] UnityEvent Skill2PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill2PressUp = new UnityEvent();
    private void OnSkill2(InputValue value)
    {
        if (value.Get<float>() == 1)//Pressed
        {
            if (Skill2PressDown != null) Skill2PressDown.Invoke();

        }
        else if (value.Get<float>() == 0)//Released
        {
            if (Skill2PressUp != null) Skill2PressUp.Invoke();
        }
    }

    [Header("Skill 3")]
    [SerializeField] UnityEvent Skill3PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill3PressUp = new UnityEvent();
    private void OnSkill3(InputValue value)
    {
        if (value.Get<float>() == 1)//Pressed
        {
            if (Skill3PressDown != null) Skill3PressDown.Invoke();

        }
        else if (value.Get<float>() == 0)//Released
        {
            if (Skill3PressUp != null) Skill3PressUp.Invoke();
        }
    }

    [Header("Skill 4")]
    [SerializeField] UnityEvent Skill4PressDown = new UnityEvent();
    [SerializeField] UnityEvent Skill4PressUp = new UnityEvent();
    private void OnSkill4(InputValue value)
    {
        if (value.Get<float>() == 1)//Pressed
        {
            if (Skill4PressDown != null) Skill4PressDown.Invoke();

        }
        else if (value.Get<float>() == 0)//Released
        {
            if (Skill4PressUp != null) Skill4PressUp.Invoke();
        }
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
