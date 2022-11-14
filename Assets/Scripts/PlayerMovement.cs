using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 1.0f;

    public Controls Controls { get; private set; }

    Rigidbody2D Rigidbody;
    Animator Animator;

    private void Awake()
    {
        Controls = new();
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        Controls.Player.Attack.performed += ctx => Attack();
    }

    private void FixedUpdate()
    {
        Movement(Controls.Player.Movement.ReadValue<Vector2>());
    }

    void Movement(Vector2 input)
    {
        Animator.SetFloat("Movement", input.magnitude);

        CheckForCharacterFlip(input.x);
        Rigidbody.MovePosition(Rigidbody.position + input * MovementSpeed * Time.deltaTime);
    }
    void CheckForCharacterFlip(float moveSpeed)
    {
        if (moveSpeed * transform.localScale.x > 0f || moveSpeed == 0) return;

        transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void Attack()
    {
        Animator.SetBool("isAttacking", true);
    }
    public void EndAttack() => Animator.SetBool("isAttacking", false);


    #region Inputs stuff
    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();
    #endregion
}
