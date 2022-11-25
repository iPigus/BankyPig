using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttackAi : MonoBehaviour
{
    EnemyMovement enemyMovement => GetComponentInParent<EnemyMovement>();

    public enum HomeDisanceForm : int
    {
        Square,
        Circle
    }

    [Header("Basic Mob Info")]
    public Vector2 HomePosition;
    [Range(0f, 100f)] public float MaxDistanceFromHome = 10f;
    public HomeDisanceForm homeDisanceForm;

    Vector2 FollowPosition => PlayerMovement.Singleton.transform.position;
    Vector2 BeforeFollowingPosition = new();
    bool isFollowingPlayer { get; set; } = false;
    bool isPlayerDetected { get; set; } = false;

    int Tick { get; set; } = 0;
    private void FixedUpdate()
    {
        Tick++;
        if (Tick % 50 == 0) return;

        if (isFollowingPlayer) FollowPlayer();

        if(sholdGoHome()) ReturnHome();
    }

    void FollowPlayer()
    {
        enemyMovement.SetPositionToGo(FollowPosition);
    }
    void StartFollowingPlayer()
    {
        if (!isFollowingPlayer) BeforeFollowingPosition = transform.position;

        isFollowingPlayer = true;
    }
    void StopFollowingPlayer() => StartCoroutine(StopFollowingAfter());
    void StopFollowingPlayer(float time) => StartCoroutine(StopFollowingAfter(time));

    void GoBackTo() => GoBackTo(BeforeFollowingPosition);
    void GoBackTo(Vector2 position)
    {
        enemyMovement.SetPositionToGo(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        StartFollowingPlayer();
        isPlayerDetected = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;  
        
        StopFollowingPlayer();
        isPlayerDetected = false;
    }

    IEnumerator StopFollowingAfter(float time = 3f)
    {
        yield return new WaitForSeconds(time);

        if (isPlayerDetected) yield break;

        GoBackTo();

        isFollowingPlayer = false;
    }

    bool sholdGoHome()
    {
        if ((HomePosition - (Vector2)transform.position).magnitude > MaxDistanceFromHome 
            && homeDisanceForm == HomeDisanceForm.Circle) return true;

        if ((Mathf.Abs(HomePosition.x - transform.position.x) > MaxDistanceFromHome || Mathf.Abs(HomePosition.y - transform.position.y) > MaxDistanceFromHome)
            && homeDisanceForm == HomeDisanceForm.Square) return true;

        return false;
    }

    void ReturnHome()
    {
        enemyMovement.SetPositionToGo(HomePosition);
    }
}
