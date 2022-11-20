using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItemSystem : MonoBehaviour
{
    public static Dictionary<string, EventItem> EventsList = new();

    public static void ClearEventList() => EventsList = new();
    public static void InvokeEvent(string eventName)
    {
        if (EventsList.TryGetValue(eventName, out EventItem item))
        {
            item.Event();
        }
        else
            Debug.LogError("Event not found!");
    }
}
