using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAi : MonoBehaviour
{
    EnemyMovement enemyMovement => GetComponentInParent<EnemyMovement>();


    Vector2 BeforeFollowingPosition = new();
    bool isFollowingPlayer { get; set; } = false;
    bool isPlayerDetected { get; set; } = false;

    int Tick { get; set; } = 0;
    private void FixedUpdate()
    {
        Tick++;
        if (Tick % 50 == 0) return;


    }

    void FollowPlayer()
    {
        if (!isFollowingPlayer) BeforeFollowingPosition = transform.position;
    }
    void StopFollowingPlayer()
    {
        GoBackTo();
    }

    void GoBackTo() => GoBackTo(BeforeFollowingPosition);
    void GoBackTo(Vector2 position)
    {
        enemyMovement.SetPositionToGo(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        FollowPlayer();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;  
        
        StopFollowingPlayer();
    }
}
