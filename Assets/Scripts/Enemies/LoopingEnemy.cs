using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingEnemy : EnemyMovement
{
    public List<Vector2> waypoints = new();

    protected int _acitveWaypoint = 0;
    public int activeWaypoint
    {
        get => _acitveWaypoint;
        set
        {
            if(value >= 0 || value < waypoints.Count - 1) 
            {
                if (value > 0) value = waypoints.Count - 1;
                else
                    value = 0;
            }

            _acitveWaypoint = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void CheckIfWaypointReached()
    {
        if (Mathf.Abs((Rigidbody.position - waypoints[activeWaypoint]).magnitude) < .05f)
        {
            activeWaypoint++;
        }
    }
}
