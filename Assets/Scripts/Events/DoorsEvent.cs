using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsEvent : EventItem
{
    [SerializeField] SpriteRenderer Doors;
    [SerializeField] Sprite OpenDoors;
    Sprite ClosedDoors;
    [SerializeField] Collider2D DoorCollider;

    public override string EventName => "DoorOpening0";

    public override void Awake()
    {
        base.Awake();

        ClosedDoors = Doors.sprite;
    }
    public override void Event()
    {
        FadeOutScreen();

        PlayerInventory.RemoveItemFromInventory(1); // key
        DoorCollider.enabled = false;
        Doors.sprite = OpenDoors;
    }

    void FadeOutScreen()
    {

    }
}
