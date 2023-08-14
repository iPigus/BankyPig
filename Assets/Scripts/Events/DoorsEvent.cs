using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsEvent : EventItem
{
    [SerializeField] SpriteRenderer Doors;
    [SerializeField] Sprite OpenDoors;
    Sprite ClosedDoors;
    [SerializeField] Collider2D DoorCollider;

    [SerializeField] string eventName; 

    public override string EventName => eventName.Length == 0 ? "default name" : eventName;

    public override void Start()
    {
        base.Start();

        ClosedDoors = Doors.sprite;
    }
    public override void Event()
    {
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        FadeInSystem.FadeIn();

        while (!FadeInSystem.isFadedIn)
        {
             yield return new WaitForEndOfFrame();
        }

        FadeInSystem.FadeOut();
        PlayerInventory.RemoveItemFromInventory(1); // key
        DoorCollider.enabled = false;
        Doors.sprite = OpenDoors;

        yield break;
    }
}
