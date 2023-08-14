using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MovementSpeed = 1;

    protected Rigidbody2D Rigidbody;
    protected Animator Animator;

    public Vector2 goToPosition = new();

    bool _isResting = false;
    bool isResting
    {
        get => _isResting;
        set
        {
            if (!value) return;
            _isResting = value;
            StartCoroutine(RestTime());
        }
    }
    float restTime = 1f;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    protected virtual void FixedUpdate()
    {
        MoveToSetPosition();

        if(PlayerHealth.isDead) StopAttacking();
    }

    public void SetPositionToGo(Vector2 goToPosition)
    {
        this.goToPosition = goToPosition;
    }
    public void SetPositionToGo(Vector2 goToPosition, float waitTime)
    {
        this.goToPosition = goToPosition;   
        restTime = waitTime;
        isResting = true;
    }

    protected void MoveToSetPosition()
    {
        if (isAnimatorAttacking || isResting) return;

        if ((goToPosition - Rigidbody.position).magnitude < 0.1f)
        {
            Animator.SetBool("isMoving", false);

            return;
        }

        Animator.SetBool("isMoving", true);

        Vector2 movement = MovementSpeed * Time.deltaTime * (goToPosition - Rigidbody.position).normalized;

        CheckForCharacterFlip(movement.x);

        Rigidbody.MovePosition(Rigidbody.position + movement);
    }
    protected void CheckForCharacterFlip(float moveSpeed)
    {
        if (moveSpeed * transform.localScale.x > 0f || moveSpeed == 0) return;

        transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    bool _isAttacking = false;
    public bool isAttacking
    {
        get => _isAttacking || isAnimatorAttacking;
        set => _isAttacking = value;
    }
    bool isAnimatorAttacking => Animator.GetBool("isAttacking");
    public virtual void StartAttacking()
    {
        isAttacking = true;
        Animator.SetBool("isAttacking", true);
    }
    public virtual void StopAttacking()
    {
        isAttacking = false;
    }
    public void AnimatorEndAttacking()
    {
        if (_isAttacking) return;

        Animator.SetBool("isAttacking", false);
    }

    IEnumerator RestTime()
    {
        yield return new WaitForSeconds(restTime);

        _isResting = false;
    }
}
