using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Singleton { get; private set; }

    public List<Item> Items = new();

    public static bool isInventoryOpen => InventorySystem.Singleton.isInventoryOpen;


    public static bool isOpenable => Singleton.Items.Count != 0;

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

        controls.Player.Interact.performed += ctx => PickUpItem();

        controls.Player.Movement.performed += ctx => MoveInInventory(ctx.ReadValue<Vector2>().x);
    }

    private void FixedUpdate()
    {
        PromptSystem.SwitchPromptState(isOpenable, "switch");
    }

    #region PickUp Systems 

    static GameObject pickableItem = null;
    static int pickableItemId;

    public static void ShowPickable(GameObject objectToPickUp, int itemId)
    {
        if (pickableItem != null) return;
        
        pickableItem = objectToPickUp;
        pickableItemId = itemId;

        PromptSystem.SwitchPromptState(true, "pickup");
    }

    public static void HidePickable(GameObject objectToPickUp, int itemId)
    {
        if(pickableItem != objectToPickUp || itemId != pickableItemId) return;

        pickableItem = null;
        pickableItemId = 0;

        PromptSystem.SwitchPromptState(false, "pickup");
    }

    public void PickUpItem()
    {
        if(pickableItem == null) return;

        PlayerInventory.AddItemToInventory(pickableItemId);
        NewItemSystem.Singleton.ShowNewItem(pickableItemId);

        Destroy(pickableItem);
    }

    #endregion

    #region ItemSystem (+end of script)

    public static void AddWeapon()
    {
        Singleton.Items.Add(AllItems.GetItemFromId(0));
    }

    float timeScaleBeforeOpening;


    void MoveInInventory(float x)
    {
        if (isInventoryOpen) InventorySystem.Singleton.MovementInInventory(x);
    }

    void OpenInventory()
    {
        if (PlayerManager.isInAnySystem || !isOpenable) return;

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

        if(activeItem >= Items.Count) activeItem = Items.Count - 1;

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
    }

    public static void AddItemToInventory(int itemId) => Singleton.Items.Add(AllItems.GetItemFromId(itemId));
    public static void RemoveItemFromInventory(int itemId) => Singleton.Items.Remove(AllItems.GetItemFromId(itemId));
    public static Item GetActiveItem()
    {
        if (Singleton.Items.Count == 0) return null;

        return Singleton.Items[Singleton.activeItem];
    }
    public static int? GetActiveItemId() => GetActiveItem()?.Id;

    public static bool doesInventoryContainItem(int itemId) => Singleton.Items.Contains(AllItems.GetItemFromId(itemId));

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

#endregion