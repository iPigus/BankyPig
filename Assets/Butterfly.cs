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

    [Header("Random Movement System")]
    public float RandomMovementRange = 5;
    public float MoveSpeed = 1f;
    [Range(0f, 1f)] public float CurveStrength = .5f;

    [Header("Max Distance")]
    [Range(0f, 100f)]public float MaxButterflyDistance = 60;

    Vector2 lastPlace;
    public Vector2 placeToGo;
    Vector2 distanceGoneInLine;

    Animator animator;

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

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ChooseNewRandomLocation(true);
    }
    private void Start()
    {
        animator.enabled = (transform.position - PlayerHealth.Singleton.transform.position).magnitude < 15f;
    }
    private void FixedUpdate()
    {
        GoToRandomPlace();

        FlipCheck();

        MoveUpAndDown();

        TooFarCheck();
    }

    void GoToRandomPlace()
    {
        if (Mathf.Abs((placeToGo - rigidbody.position).magnitude) < .1f)
        {
            ChooseNewRandomLocation();
        }

        if (isResting) return;

        Vector2 OriginalDistance = placeToGo - lastPlace;

        Vector2 Movement = CurveStrength * ScaleVectorToCurve(lastPlace, placeToGo, distanceGoneInLine) + (1 - CurveStrength) * OriginalDistance.normalized;

        distanceGoneInLine += OriginalDistance.normalized * Time.deltaTime;
        rigidbody.position += Movement * Time.deltaTime;
    }

    void ChooseNewRandomLocation(bool isFromStart = false)
    {
        if (!isFromStart) isResting = true;

        lastPlace = transform.position;
        distanceGoneInLine = transform.position;
        placeToGo = new(Random.Range(-RandomMovementRange, RandomMovementRange) + transform.position.x, Random.Range(-RandomMovementRange, RandomMovementRange) + transform.position.y);
    }

    Vector2 ScaleVectorToCurve(Vector2 startPos, Vector2 endPos, Vector2 placeToScale)
    {
        Vector2 Curve = endPos - startPos;

        float progressX = ((placeToScale.x / Curve.x) - 1) * Mathf.PI / 2;
        float progressY = ((placeToScale.x / Curve.y) - 1) * Mathf.PI / 2;

        return new(Mathf.Cos(progressX), Mathf.Sin(progressY) + 1); 
    }

    IEnumerator RestTime(float time = 2f)
    {
        yield return new WaitForSeconds(time);

        _isResting = false;
    }

    void MoveUpAndDown()
    {
        tick++;
        float movement = verticalSpeed * Time.deltaTime * (isMovingDown ? -1 : 1);

        movementY += movement;

        rigidbody.MovePosition(new(rigidbody.position.x, rigidbody.position.y + movement));

        if(tick % 8 == 0) isMovingDown = !isMovingDown;
    }

    Vector3 lastPosition = new();
    void FlipCheck()
    {
        if ((transform.position - lastPosition).x * transform.localScale.x < 0) transform.localScale = new(-transform.localScale.x, transform.localScale.y);

        lastPosition = transform.position;
    }

    void TooFarCheck()
    {
        if (tick % 2 == 0) return;

        if (Mathf.Abs(transform.position.x) > MaxButterflyDistance || Mathf.Abs(transform.position.y) > MaxButterflyDistance)
        {
            ButterflySystem.SpawnButterfly();

            ButterflySystem.Singleton.Butterflies.Remove(gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        animator.enabled = false; 
    }
    private void OnBecameVisible()
    {
        animator.enabled = true; 
    }
}
