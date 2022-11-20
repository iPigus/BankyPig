using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
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
        EventItemSystem.EventsList.Add(EventName, this);
    }
}
