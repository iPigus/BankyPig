using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyDog : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;

    public Vector2 pointToEscape;
    public float speed = 5f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void RunFromPlayer() => StartCoroutine(RunningFromPlayer());
    IEnumerator RunningFromPlayer()
    {
        if ((rigidbody.position - pointToEscape).magnitude < .1f)
        {
            transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        while ((rigidbody.position - pointToEscape).magnitude > .1f)
        {
            Vector2 movement = pointToEscape - rigidbody.position; 
            movement = movement.normalized * Time.deltaTime * speed;

            rigidbody.MovePosition(rigidbody.position + movement);

            CheckForCharacterFlip(movement.normalized.x);
            animator.SetBool("isMoving", true);

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isMoving", false);
    }

    protected void CheckForCharacterFlip(float moveSpeed)
    {
        if (moveSpeed * transform.localScale.x > 0f || moveSpeed == 0) return;

        transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
