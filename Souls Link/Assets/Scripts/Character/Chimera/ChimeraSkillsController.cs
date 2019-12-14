using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChimeraSkillsController : PlayerSkills
{
    [SerializeField] private ChimeraSkillCombo[] combos;

    public void sendSkillV2(GameManager.Characters typeCharacter, float pressValue, float skillIndex)
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

        //_pushValueSkill(value, index);

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

    public void sendSkill(PlayerSkills.SkillType skillReceived, float pressValue)
    {
        switch (skillReceived)
        {
            case PlayerSkills.SkillType.ICE_STAKE:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<StakeAttack>())
                    {
                        GetComponent<StakeAttack>().shotStake();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.REFFLECT:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<ReflectAttack>())
                    {
                        GetComponent<ReflectAttack>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.DECOY:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Decoy>())
                    {
                        GetComponent<Decoy>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.RANDOM_GUN:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<RandomGun>())
                    {
                        GetComponent<RandomGun>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.ROOT:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Root>())
                    {
                        GetComponent<Root>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                    if (GetComponent<Root>())
                    {
                        GetComponent<Root>().unPressKey();
                    }
                }
                break;

            case PlayerSkills.SkillType.VINE_WHIP:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<VineWhip>())
                    {
                        GetComponent<VineWhip>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.WALL_SKILL:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<WallSkill>())
                    {
                        GetComponent<WallSkill>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.POSION_DART:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<PoisonDart>())
                    {
                        GetComponent<PoisonDart>().shootPosionDart();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.STRONG_ATTACK:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<StrongAttack>())
                    {
                        GetComponent<StrongAttack>().attack();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.TORNADO_MINE:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<TornadoMine>())
                    {
                        GetComponent<TornadoMine>().spawnBomb();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.DASH:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Dash>())
                    {
                        GetComponent<Dash>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                    if (GetComponent<Dash>())
                    {
                        GetComponent<Dash>().unPressKey();
                    }
                }
                break;

            case PlayerSkills.SkillType.MIST:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Mist>())
                    {
                        GetComponent<Mist>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {

                }
                break;

            case PlayerSkills.SkillType.FLAMETHROWER:


            case PlayerSkills.SkillType.BOOMERANG:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Boomerang>())
                    {
                        GetComponent<Boomerang>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.HOOK:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<Hook>())
                    {
                        GetComponent<Hook>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.MAGMA_RING:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<MagmaRing>())
                    {
                        GetComponent<MagmaRing>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                }
                break;

            case PlayerSkills.SkillType.SMASH_ATTACK:
                if (pressValue == 1)//Pressed
                {
                    if (GetComponent<SmashAttack>())
                    {
                        GetComponent<SmashAttack>().pressKey();
                    }
                }
                else if (pressValue == 0)//Released
                {
                    if (GetComponent<SmashAttack>())
                    {
                        GetComponent<SmashAttack>().unPressKey();
                    }
                }
                break;
        }
    }
}
