using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    new Rigidbody2D rigidbody;

    public float verticalSpeed = .05f;
    public float movementY = 0;
    public float movementYrange = 0.2f;
    public bool isMovingDown = false;

    int tick = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        GoToRandomPlace();

        MoveUpAndDown();
    }

    void GoToRandomPlace()
    {

    }

    void MoveUpAndDown()
    {
        tick++;
        float movement = verticalSpeed * Time.deltaTime * (isMovingDown ? -1 : 1);

        movementY += movement;

        rigidbody.MovePosition(new(rigidbody.position.x, rigidbody.position.y + movement));

        if(tick % 8 == 0) isMovingDown = !isMovingDown;
    }
}
