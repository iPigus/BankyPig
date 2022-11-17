using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Singleton { get; private set; }

    public List<Item> Items = new();

    Controls controls;

    private void Awake()
    {
        Singleton = this;

        LoadInventory();

        controls.Player.ShowInventory.performed += ctx => OpenInventory();
        controls.Player.ShowInventory.canceled += ctx => CloseInventory();
    }              

    void OpenInventory()
    {
        InventorySystem.Singleton.OpenInventory();
    }
    void CloseInventory()
    {
        InventorySystem.Singleton.CloseInventory();
    }


    void LoadInventory()
    {
        Items = new();

        Items.Add(AllItems.GetItemFromId(0));
    }

    #region Input Stuff
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    #endregion
}

public class Item
{
    public Item(Sprite sprite, string name, string describtion, int id)
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
public static class AllItems
{
    public static Item GetItemFromId(int id)
    {
        if(ItemList.TryGetValue(id, out var item)) return item;
        else
            return null;
    }

    public static Dictionary<int, Item> ItemList = new();

    static AllItems()
    {
        for (int i = 0; i < ItemData.ItemSprites.Count; i++)
        {
            ItemList.Add(ItemList.Count, new(ItemData.ItemSprites[i], ItemData.ItemNames[i], ItemData.ItemDescribtions[i], ItemList.Count));
        }
    }
}