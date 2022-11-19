using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Singleton { get; private set; }

    public List<Item> Items = new();

    public static bool isInventoryOpen => InventorySystem.Singleton.isInventoryOpen;

    public int activeItem
    {
        get => InventorySystem.Singleton.activeItem;
        set => InventorySystem.Singleton.activeItem = value;
    }

    Controls controls;

    private void Awake()
    {
        Singleton = this;
        controls = new();

        LoadInventory();

        controls.Player.ShowInventory.performed += ctx => OpenInventory();
        controls.Player.ShowInventory.canceled += ctx => CloseInventory();

        controls.Player.Movement.performed += ctx => MoveInInventory(ctx.ReadValue<Vector2>().x);
    }

    float timeScaleBeforeOpening;


    void MoveInInventory(float x)
    {
        if (isInventoryOpen) InventorySystem.Singleton.MovementInInventory(x);
    }

    void OpenInventory()
    {
        if (PlayerMovement.Singleton.isAttacking || NewItemSystem.isNewItemSystemActive) return;

        CheckIfActiveItemIsEquippable();

        InventorySystem.Singleton.OpenInventory();

        timeScaleBeforeOpening = Time.timeScale;

        Time.timeScale /= 5f;
    }
    void CloseInventory()
    {
        if (!isInventoryOpen) return;

        CheckIfActiveItemIsEquippable();

        InventorySystem.Singleton.CloseInventory();

        Time.timeScale = timeScaleBeforeOpening;
        
        timeScaleBeforeOpening = 0;
    }

    void CheckIfActiveItemIsEquippable()
    {
        if (Items.Count == 0) return;

        if (Items[activeItem].IsEquippable || !Items.Where(x => x.IsEquippable).Any()) return;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsEquippable)
            {
                activeItem = i;

                break;
            }
        }
    }


    void LoadInventory()
    {
        Items = new();

        Items.Add(AllItems.GetItemFromId(0));
    }

    public void AddItemToInventory(int itemId) => Items.Add(AllItems.GetItemFromId(itemId));

    public bool doesInventoryContainItem(int itemId) => Items.Contains(AllItems.GetItemFromId(itemId));

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
    public Item(Sprite sprite, string name, string describtion, int id, bool isEquippable)
    {
        Sprite = sprite;
        Name = name;
        Describtion = describtion;
        Id = id;
        IsEquippable = isEquippable;
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Describtion { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool IsEquippable { get; private set;}

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
            ItemList.Add(ItemList.Count, new(ItemData.ItemSprites[i], ItemData.ItemNames[i], ItemData.ItemDescribtions[i], ItemList.Count, ItemData.IsItemEquippable[i]));
        }
    }
}