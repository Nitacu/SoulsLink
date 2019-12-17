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

    public void setSkill(bool active, int skillIndex, float _readingTime)
    {      

        skillIndex -= 1;

        skills[skillIndex].SetActive(active);
        if (active)
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
