using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public void SetItemStats(Sprite sprite, string name, string describtion, int id)
    {
        Sprite = sprite;
        Name = name;
        Describtion = describtion;
        Id = id;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Describtion { get; private set; }
    public Sprite Sprite { get; private set; }
}
