using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public void SetItemStats(Sprite sprite, string name, string describtion, int id, bool isItemEquippable)
    {
        Sprite = sprite;
        Name = name;
        Describtion = describtion;
        Id = id;
        IsItemEquippable = isItemEquippable;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Describtion { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool IsItemEquippable { get; private set; }
}
