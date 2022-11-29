using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyEvent : EventItem
{
    public override string EventName => "OldLadyDog";

    public override void Event()
    {
        PlayerInventory.RemoveItemFromInventory(3);

        PlayerInventory.AddItemToInventory(4);
        NewItemSystem.Singleton.ShowNewItem(4);

        GetComponent<CharacterPointMovement>().enabled = false;
        GameObject Dog = new();
        Dog.transform.position = transform.position + new Vector3(1, 0, 0);
        Dog.AddComponent<SpriteRenderer>().sprite = ItemData.ItemSprites[3];
    }
}
