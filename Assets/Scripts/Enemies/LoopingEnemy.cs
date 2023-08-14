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
            if(value <= 0 || value > waypoints.Count - 1) 
            {
                if (value > 0) value = waypoints.Count - 1;
                else
                    value = 0;
            }

            _acitveWaypoint = value;
        }
    }

    public float waypointWaitTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        SetPositionToGo(waypoints[activeWaypoint]);

        CheckForWaypointCount();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckIfWaypointReached();
    }

    void CheckIfWaypointReached()
    {
        if (Mathf.Abs((Rigidbody.position - waypoints[activeWaypoint]).magnitude) < .1f)
        {
            if (activeWaypoint == waypoints.Count - 1)
                activeWaypoint = 0;
            else 
                activeWaypoint++;

            if(waypointWaitTime <= 0f)
                SetPositionToGo(waypoints[activeWaypoint]);
            else
                SetPositionToGo(waypoints[activeWaypoint], waypointWaitTime);
        }
    }

    public void GoToActiveWaypoint() => SetPositionToGo(waypoints[activeWaypoint]);
    void CheckForWaypointCount()
    {
#if UNITY_EDITOR
        if (waypoints.Count == 0) Debug.LogError("No waypoints set!");
#endif
    }
}
