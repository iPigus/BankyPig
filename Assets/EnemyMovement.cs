using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MovementSpeed = 1;

    Rigidbody2D Rigidbody;
    Animator Animator;

    public Vector2 goToPosition = new();

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        MoveToSetPosition();
    }

    public void SetPositionToGo(Vector2 goToPosition)
    {
        this.goToPosition = goToPosition;
    }

    void MoveToSetPosition()
    {
        if ((goToPosition - Rigidbody.position).magnitude < 0.05f)
        {
            Animator.SetBool("isMoving", false);

            return;
        }

        Animator.SetBool("isMoving", true);

        Vector2 movement = (goToPosition - Rigidbody.position).normalized * MovementSpeed * Time.deltaTime;

        CheckForCharacterFlip(movement.x);

        Rigidbody.MovePosition(Rigidbody.position + movement);
    }
    void CheckForCharacterFlip(float moveSpeed)
    {
        if (moveSpeed * transform.localScale.x > 0f || moveSpeed == 0) return;

        transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
