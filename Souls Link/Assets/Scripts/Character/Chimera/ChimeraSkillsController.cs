using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChimeraSkillsController : PlayerSkills
{
    #region Delegate
    public delegate bool DelegateMultiplayerSkillController();
    public DelegateMultiplayerSkillController _isMine;
    #endregion

    [SerializeField] private ChimeraSkillCombo[] combos;

    public void sendSkill(GameManager.Characters typeCharacter, float pressValue, float skillIndex)
    {
        Debug.Log("send Skill v2 RECIBIO");

        //Ver chimera type para saber qué orden de la Skill Mandar
        List<GameManager.Characters> chimeraTypes = GetComponent<ChimeraTypes>()._types;

        for (int i = 0; i < chimeraTypes.Count; i++)
        {
            if (chimeraTypes[i] == typeCharacter)
            {
                Debug.Log("TIPO ENCONTRADO, LANZAR INDEX SKILL");
                indexSkill(i, pressValue, skillIndex);
                break;
            }
        }
    }

    private void indexSkill(int index, float pressValue, float skillIndex)
    {
        switch (index)
        {
            case 0://0 a 4                   
                switch (skillIndex)
                {
                    case 0:
                        Debug.Log("SKILL 1");
                        StartCoroutine(skillDelay(pressValue, Skill1PressDown, Skill1PressUp, skillIndex));
                        break;
                    case 1:
                        Debug.Log("SKILL 2");
                        StartCoroutine(skillDelay(pressValue, Skill2PressDown, Skill2PressUp, skillIndex));
                        break;
                    case 2:
                        Debug.Log("SKILL 3");
                        StartCoroutine(skillDelay(pressValue, Skill3PressDown, Skill3PressUp, skillIndex));
                        break;
                    case 3:
                        Debug.Log("SKILL 4");
                        StartCoroutine(skillDelay(pressValue, Skill4PressDown, Skill4PressUp, skillIndex));
                        break;
                    default:
                        break;
                }
                break;

            case 1://4 a 8
                switch (skillIndex)
                {
                    case 0:
                        Debug.Log("SKILL 5");
                        StartCoroutine(skillDelay(pressValue, Skill5PressDown, Skill5PressUp, skillIndex));
                        break;
                    case 1:
                        Debug.Log("SKILL 6");
                        StartCoroutine(skillDelay(pressValue, Skill6PressDown, Skill6PressUp, skillIndex));
                        break;
                    case 2:
                        Debug.Log("SKILL 7");
                        StartCoroutine(skillDelay(pressValue, Skill7PressDown, Skill7PressUp, skillIndex));
                        break;
                    case 3:
                        Debug.Log("SKILL 8");
                        StartCoroutine(skillDelay(pressValue, Skill8PressDown, Skill8PressUp, skillIndex));
                        break;
                    default:
                        break;
                }
                break;
            case 2://9 a 12
                switch (skillIndex)
                {
                    case 0:
                        StartCoroutine(skillDelay(pressValue, Skill9PressDown, Skill9PressUp, skillIndex));
                        break;
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill10PressDown, Skill10PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill11PressDown, Skill11PressUp, skillIndex));
                        break;
                    case 3:
                        StartCoroutine(skillDelay(pressValue, Skill12PressDown, Skill12PressUp, skillIndex));
                        break;
                    default:
                        break;
                }

                break;
            case 3://13 a 16
                switch (skillIndex)
                {
                    case 0:
                        StartCoroutine(skillDelay(pressValue, Skill13PressDown, Skill13PressUp, skillIndex));
                        break;
                    case 1:
                        StartCoroutine(skillDelay(pressValue, Skill14PressDown, Skill14PressUp, skillIndex));
                        break;
                    case 2:
                        StartCoroutine(skillDelay(pressValue, Skill15PressDown, Skill15PressUp, skillIndex));
                        break;
                    case 3:
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
