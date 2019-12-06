using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmEnemyAttack : MonoBehaviour
{
    public void doAttack()
    {
        transform.parent.gameObject.transform.parent.gameObject.GetComponentInParent<SimpleEnemyController>().checkIfDamages();
    }
}
