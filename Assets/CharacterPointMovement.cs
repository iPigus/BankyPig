using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterPointMovement : MonoBehaviour
{
    Animator animator;
    new Rigidbody2D rigidbody;
    Collider2D[] collieders;

    [SerializeField] List<Vector2> points = new();
    [Range(.1f, 10f)] public float movementSpeed = 1f;
    [Range(0f, 10f)] public float stopTime = 1f;
    public bool shouldLoop = false;
    public bool shouldPushOnMovement = false;
    int activeIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collieders = GetComponents<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (CheckIfPointReached()) IndexUp();

        if (!shouldPushOnMovement) ChangeCollidersOnMoveOrStay(isStopped);

        if (!isStopped)
            Move();
    }

    bool CheckIfPointReached()
    {
        if (points.Count == 0) return true;

        if ((points[activeIndex] - rigidbody.position).magnitude < .1f) return true;

        return false;
    }

    bool isStopped = false;
    bool isIndexGoingUp = true;
    void IndexUp()
    {
        if (shouldLoop)
        {
            activeIndex++;

            if (activeIndex == points.Count) activeIndex = 0;
        }
        else
        {
            if (isIndexGoingUp)
            {
                if (activeIndex == points.Count - 1)
                {
                    activeIndex--;
                    isIndexGoingUp = false;
                }
                else 
                    activeIndex++;
            }
            else
            {
                if (activeIndex == 0)
                {
                    activeIndex++;
                    isIndexGoingUp = true;
                }
                else
                    activeIndex--;
            }
        }

        if (stopTime > 0) StartCoroutine(WaitBeforeNextPoint());
    }

    IEnumerator WaitBeforeNextPoint()
    {
        isStopped = true;

        yield return new WaitForSeconds(stopTime);

        isStopped = false;
    }

    void ChangeCollidersOnMoveOrStay(bool shouldPush)
    {
        if (!collieders.Where(x => !x.isTrigger).Any()) return;

        collieders.Where(x => !x.isTrigger).ToList().ForEach(x => x.enabled = shouldPush);
    }

    void Move()
    {
        if(points.Count == 0) return;

        Vector2 movement = (rigidbody.position - points[activeIndex]).normalized * Time.deltaTime * movementSpeed;

        rigidbody.MovePosition(rigidbody.position - movement);
    }
}
