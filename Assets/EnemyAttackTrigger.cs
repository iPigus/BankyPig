using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        enemyMovement.StartAttacking();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; 
        
        enemyMovement.StopAttacking();
    }
}
