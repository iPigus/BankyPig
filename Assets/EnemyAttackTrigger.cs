using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    EnemyMovement enemyMovement;

    public float AttackWaitTime = .2f;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        StartCoroutine(Attack());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        StartCoroutine(StopAttacking());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(AttackWaitTime);

        enemyMovement.StartAttacking();
    }
    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(AttackWaitTime);

        enemyMovement.StopAttacking();
    }
}
