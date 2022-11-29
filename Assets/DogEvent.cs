using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEvent : EventItem
{
    public override string EventName => "PickUpDog0";

    public override void Event()
    {
        PlayerInventory.RemoveItemFromInventory(2);

        PlayerInventory.AddItemToInventory(3);
        NewItemSystem.Singleton.ShowNewItem(3);

        Destroy(gameObject);
    }
}
