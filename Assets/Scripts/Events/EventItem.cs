using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EventItem : MonoBehaviour
{
    public abstract string EventName { get; }
    public abstract void Event();

    public virtual void Awake()
    {
        AddToEventList();   
    }
    public virtual void AddToEventList()
    {
        if (EventItemSystem.EventsList.Keys.Contains(EventName))
        {
            Debug.Log("Event already added, name: " + EventName);
            return;
        }

        EventItemSystem.EventsList.Add(EventName, this);
    }
}
