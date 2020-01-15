using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraSkillFeedBack : ChimeraArrow
{
    private const float SCALE_BASE = 0.4f;

    [SerializeField] GameObject[] skills = new GameObject[4];
    private int[] inputSkillPressed = new int[4] {0,0,0,0 };
    private float activeCooldown = 1;
    public float currentActiveCooldown = 0;


    public enum FeedBackType
    {
        INDIVUAL,
        GENERAL
    }
    [SerializeField] FeedBackType _feedBacktype;

    private void Update()
    {
        if (currentActiveCooldown > 0)
        {
            currentActiveCooldown -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void setSkill(float active, int skillIndex, float _readingTime)
    {
        bool isActive = true;

        if (active == 1)//Pressed
            isActive = true;

        skillIndex -= 1;

        if (isActive)
        {
            if (_feedBacktype == FeedBackType.GENERAL)
            {
                //PARA QUE EL FEEDBACK SEA EL INPUT AGRANDANDOSE POR CADA PRESIONADO

                //saber cuantos players hay para sacar porcentajes
                int numberPlayersInChimera = GetComponentInParent<ChimeraController>().Players.Count;

                float factorScale = SCALE_BASE / numberPlayersInChimera;

                //saber actualmente cuantos estan presionando
                int numberPressed = inputSkillPressed[skillIndex];
                numberPressed++;

                //cambiar escala con los que estan presionando
                float newScale = factorScale * numberPressed;
                Vector2 newVectorScale = new Vector2(newScale, newScale);

                skills[skillIndex].transform.localScale = newVectorScale;

                skills[skillIndex].SetActive(isActive);
                currentActiveCooldown = activeCooldown;
                StartCoroutine(deactiveGeneralSkill(skillIndex, _readingTime));

            }
            else
            {
                //PARA QUE EL FEEDBACK SEA ACTIVADO POR COLORES DEPENDIENDO EL PERSONAJE
                skills[skillIndex].SetActive(isActive);
                currentActiveCooldown = activeCooldown;
                StartCoroutine(deactiveSkill(skillIndex, _readingTime));
            }
        }
    }


    IEnumerator deactiveSkill(int skillIndex, float _readingTime)
    {
        yield return new WaitForSeconds(_readingTime);

        skills[skillIndex].SetActive(false);
    }

    IEnumerator deactiveGeneralSkill(int skillIndex, float _readingTime)
    {
        yield return new WaitForSeconds(_readingTime);

        skills[skillIndex].transform.localScale = Vector2.zero;
        skills[skillIndex].SetActive(false);
    }

    private void OnDisable()
    {
        foreach (var item in skills)
        {
            item.SetActive(false);
        }
    }
}
