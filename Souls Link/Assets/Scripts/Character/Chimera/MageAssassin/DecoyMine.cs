using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyMine : MineController
{
    [SerializeField] private float _castRadius = 1;

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position, _castRadius);

        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                applyDamage(hit.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Color.red;
        color.a = 0.4f;
        Gizmos.color = color;
        Gizmos.DrawSphere(gameObject.transform.position, _castRadius);
    }
}
