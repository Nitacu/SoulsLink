using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraSkillFeedBack : ChimeraArrow
{
    [SerializeField] GameObject[] skills = new GameObject[4];
    private float activeCooldown = 1;
    public float currentActiveCooldown = 0;

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
        else if (active == 0)//Released
            isActive = false;


        skillIndex -= 1;

        skills[skillIndex].SetActive(isActive);
        if (isActive)
        {
            currentActiveCooldown = activeCooldown;
            StartCoroutine(deactiveSkill(skillIndex, _readingTime));
        }
    }

    IEnumerator deactiveSkill(int skillIndex, float _readingTime)
    {
        yield return new WaitForSeconds(_readingTime);

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
