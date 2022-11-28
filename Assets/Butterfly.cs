using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    new Rigidbody2D rigidbody;

    public float verticalSpeed = 10f;
    public float movementY = 0;
    public float movementYrange = 0.2f;
    public bool isMovingDown = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {


        MoveUpAndDown();
    }
    void MoveUpAndDown()
    {
        float movement = verticalSpeed * Time.deltaTime * (isMovingDown ? -1 : 1);

        movementY += movement;

        rigidbody.MovePosition(new(rigidbody.position.x, rigidbody.position.y + movement));

        if (movementY > movementYrange) isMovingDown = false;
        if (-movementY < -movementYrange) isMovingDown = true;
    }
}
