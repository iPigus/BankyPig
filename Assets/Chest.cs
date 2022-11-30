using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : MonoBehaviour
{
    static Chest _Singleton = null;
    public static Chest Singleton
    {
        get => _Singleton;
        set
        {
            _Singleton = value;
        }
    }

    Controls controls;

    private void Awake()
    {
        controls = new();

        controls.Player.Interact.performed += ctx => TryOpenChest();
    }

    public bool isLocked = false;
    public int keyItemId = 0;
    public int itemInChestId = 0;
    public bool shouldDeleteKeyItem = true;
    public bool haveChestBeenOpened = false;
    public bool isActive { get; set; } = false;

    void TryOpenChest()
    {
        if (PlayerManager.isInAnySystem || Singleton == null) return;

        if (Singleton.isLocked)
        {
            if (PlayerInventory.doesInventoryContainItem(Singleton.keyItemId)) ChestOpened();
            else 
                CannotOpenChest();
        }
        else ChestOpened();
    }

    void CannotOpenChest() // have to throw some error
    {
        Debug.LogError("Cannot open chest!");


    }

    #region Chest Activation

    public static void CheckForActiveChests()
    {
        Chest[] chests = FindObjectsOfType<Chest>();

        chests.Where(x => x.isActive).ToList().ForEach(x => x.DeactivateChest());
    }

    void ActivateChest()
    {
        isActive = true;
    }
    void DeactivateChest()
    {
        isActive = false;
    }

    #endregion

    void ChestOpened() // new item system I guess
    {
        Debug.LogError("Chest opened!");

        if (shouldDeleteKeyItem) PlayerInventory.RemoveItemFromInventory(keyItemId);

        NewItemSystem.Singleton.ShowNewItem(itemInChestId);
        PlayerInventory.AddItemToInventory(itemInChestId);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Singleton == null || !collision.CompareTag("Player")) return;

        Singleton.ActivateChest();

        Singleton = this;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Singleton == null) return;

        if (!Singleton.Equals(this) || !collision.CompareTag("Player")) return;

        Singleton.DeactivateChest();

        Singleton = null;
    }

    #region Input stuff
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
