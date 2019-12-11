using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraSkillsController : MonoBehaviour
{
    [SerializeField] private ChimeraSkillCombo[] combos;

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
