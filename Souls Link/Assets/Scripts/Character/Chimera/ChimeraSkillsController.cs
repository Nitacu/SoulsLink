using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputSkill
{
    public InputSkill()
    {
        _state = false;
        _currentCooldown = 0;
    }

    public bool _state;
    public float _currentCooldown;
}

public class ChimeraSkillsController : PlayerSkills
{
    #region Delegate
    public delegate bool DelegateMultiplayerSkillController();
    public DelegateMultiplayerSkillController _isMine;
    #endregion

    #region InputPairs
    public InputSkill[] _inputSkill1;
    public InputSkill[] _inputSkill2;
    public InputSkill[] _inputSkill3;
    public InputSkill[] _inputSkill4;
    #endregion

    [SerializeField] private float _inputSkillReadingTime = 1f;

    [SerializeField] private List<GameObject> inputFeedBack = new List<GameObject>();

    private void Start()
    {
        foreach (var item in inputFeedBack)
        {
            item.SetActive(false);
        }
    }

    private void Update()
    {
        //cooldown handle
        foreach (var inputSlot in _inputSkill1)
        {
            setInput(inputSlot);
        }
        foreach (var inputSlot in _inputSkill2)
        {
            setInput(inputSlot);
        }
        foreach (var inputSlot in _inputSkill3)
        {
            setInput(inputSlot);
        }
        foreach (var inputSlot in _inputSkill4)
        {
            setInput(inputSlot);
        }

        //Lanzar habilidades
        if (canLaunchSkill(_inputSkill1))
        {
            StartCoroutine(skillDelay(1, Skill1PressDown, null, 1));
            resetInputSkillOnLaunch(_inputSkill1);
        }
        if (canLaunchSkill(_inputSkill2))
        {
            StartCoroutine(skillDelay(1, Skill2PressDown, null, 2));
            resetInputSkillOnLaunch(_inputSkill2);
        }
        if (canLaunchSkill(_inputSkill3))
        {
            StartCoroutine(skillDelay(1, Skill3PressDown, null, 3));
            resetInputSkillOnLaunch(_inputSkill3);
        }
        if (canLaunchSkill(_inputSkill4))
        {
            StartCoroutine(skillDelay(1, Skill4PressDown, null, 4));
            resetInputSkillOnLaunch(_inputSkill4);
        }
    }

    public void setChimeraSize(int chimeraSize)
    {
        _inputSkill1 = new InputSkill[chimeraSize];
        setInputArrayObjects(_inputSkill1);

        _inputSkill2 = new InputSkill[chimeraSize];
        setInputArrayObjects(_inputSkill2);


        _inputSkill3 = new InputSkill[chimeraSize];
        setInputArrayObjects(_inputSkill3);

        _inputSkill4 = new InputSkill[chimeraSize];
        setInputArrayObjects(_inputSkill4);
    }

    private void setInputArrayObjects(InputSkill[] inputSkills)
    {

        for (int i = 0; i < inputSkills.Length; i++)
        {
            inputSkills[i] = new InputSkill();
        }
    }

    private void resetInputSkillOnLaunch(InputSkill[] inputSkills)
    {
        foreach (var inputSkill in inputSkills)
        {
            inputSkill._state = false;
        }
    }

    private bool canLaunchSkill(InputSkill[] inputSkills)
    {
        bool launchSkill = true;
        foreach (var inputSkill in inputSkills)
        {
            if (inputSkill == null)
            {
                launchSkill = false;
                break;
            }

            if (!inputSkill._state)
            {
                launchSkill = false;
            }
        }

        return launchSkill;
    }

    private void setInput(InputSkill inputSlot)
    {
        if (inputSlot._state)//si esta presionado y es mayor que cero reducir cooldown
        {
            if (inputSlot._currentCooldown > 0)
            {
                inputSlot._currentCooldown -= Time.deltaTime;
            }
            else
            {
                inputSlot._state = false;
            }
        }
    }

    private void setArrayValue(float pressValue, int ID, InputSkill[] inputSkill, int skillIndex, GameManager.Characters characterType)
    {
        if (pressValue == 1)//Pressed
        {
            //hacer true al instante
            Debug.Log("Set array value index: " + skillIndex);
            if (ID <= inputSkill.Length - 1)
            {
                if (inputSkill[ID] != null)
                {
                    Debug.Log("INPUT SKILL VACIO: " + false);
                }
                else
                {
                    Debug.Log("INPUT SKILL VACIO: " + true);
                }

                inputSkill[ID]._state = true;
                inputSkill[ID]._currentCooldown = _inputSkillReadingTime;
            }


        }
        else if (pressValue == 0)//Released
        {
            //esperar un tiempo para hacer false
            //inputSkill[ID]._state = false;
        }
    }

    private void setInputSkillArrays(float pressValue, float skillIndex, int ID, GameManager.Characters _characteryType)
    {
        switch (skillIndex)
        {
            case 1:
                setArrayValue(pressValue, ID, _inputSkill1, 1, _characteryType);
                break;
            case 2:
                setArrayValue(pressValue, ID, _inputSkill2, 2, _characteryType);
                break;
            case 3:
                setArrayValue(pressValue, ID, _inputSkill3, 3, _characteryType);
                break;
            case 4:
                setArrayValue(pressValue, ID, _inputSkill4, 4, _characteryType);
                break;
            default:
                break;
        }
    }

    public void setSkillFeedback(GameManager.Characters typeCharacter, int skillIndex, float active)
    {
        if (inputFeedBack.Count > 0)
        {
            foreach (var arrow in inputFeedBack)
            {
                ChimeraSkillFeedBack chimeraSkillFeedBack = arrow.GetComponent<ChimeraSkillFeedBack>();

                if (chimeraSkillFeedBack.CharacterType == typeCharacter)
                {
                    chimeraSkillFeedBack.gameObject.SetActive(true);
                    chimeraSkillFeedBack.setSkill(active, skillIndex, _inputSkillReadingTime);
                }
            }
        }
    }

    public void sendSkill(GameManager.Characters typeCharacter, float pressValue, float skillIndex, int ID)
    {
        setInputSkillArrays(pressValue, skillIndex, ID, typeCharacter);
        return;

        //Ver chimera type para saber qué orden de la Skill Mandar
        List<GameManager.Characters> chimeraTypes = GetComponent<ChimeraTypes>()._types;

        for (int i = 0; i < chimeraTypes.Count; i++)
        {
            if (chimeraTypes[i] == typeCharacter)
            {
                indexSkill(i, pressValue, skillIndex);
                break;
            }
        }
    }

    private void indexSkill(int index, float pressValue, float skillIndex)
    {
        if (skillIndex == 0)//dash
        {
            StartCoroutine(skillDelay(pressValue, DashPressDown, null, skillIndex));
            return;
        }

        switch (index)
        {
            case 0://0 a 4                   
                switch (skillIndex)
                {
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill1PressDown, Skill1PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill2PressDown, Skill2PressUp, skillIndex));
                        break;
                    case 3:
                        StartCoroutine(skillDelay(pressValue, Skill3PressDown, Skill3PressUp, skillIndex));
                        break;
                    case 4:
                        StartCoroutine(skillDelay(pressValue, Skill4PressDown, Skill4PressUp, skillIndex));
                        break;
                    default:
                        break;
                }
                break;

            case 1://4 a 8
                switch (skillIndex)
                {
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill5PressDown, Skill5PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill6PressDown, Skill6PressUp, skillIndex));
                        break;
                    case 3:
                        StartCoroutine(skillDelay(pressValue, Skill7PressDown, Skill7PressUp, skillIndex));
                        break;
                    case 4:
                        StartCoroutine(skillDelay(pressValue, Skill8PressDown, Skill8PressUp, skillIndex));
                        break;
                    default:
                        break;
                }
                break;
            case 2://9 a 12
                switch (skillIndex)
                {
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill9PressDown, Skill9PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill10PressDown, Skill10PressUp, skillIndex));
                        break;
                    case 3:
                        StartCoroutine(skillDelay(pressValue, Skill11PressDown, Skill11PressUp, skillIndex));
                        break;
                    case 4:
                        StartCoroutine(skillDelay(pressValue, Skill12PressDown, Skill12PressUp, skillIndex));
                        break;
                    default:
                        break;
                }

                break;
            case 3://13 a 16
                switch (skillIndex)
                {
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill13PressDown, Skill13PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill14PressDown, Skill14PressUp, skillIndex));
                        break;
                    case 3:
                        StartCoroutine(skillDelay(pressValue, Skill15PressDown, Skill15PressUp, skillIndex));
                        break;
                    case 4:
                        StartCoroutine(skillDelay(pressValue, Skill16PressDown, Skill16PressUp, skillIndex));
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator skillDelay(float value, UnityEvent _eventDown, UnityEvent _eventUp, float index)
    {
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
