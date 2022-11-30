using System;
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

        animator = GetComponent<Animator>();
        SetOpenedState();

        controls.Player.Interact.performed += ctx => TryOpenChest();
    }

    public bool isLocked = false;
    public int keyItemId = 0;
    public int itemInChestId = 0;
    public bool shouldDeleteKeyItem = true;
    public bool haveChestBeenOpened = false;
    public bool isActive { get; set; } = false;

    Animator animator { get; set; }

    void TryOpenChest()
    {
        if (PlayerManager.isInAnySystem || Singleton == null) return;

        if (Singleton.isLocked)
        {
            if (PlayerInventory.doesInventoryContainItem(Singleton.keyItemId)) Singleton.ChestOpened();
            else 
                Singleton.CannotOpenChest();
        }
        else Singleton.ChestOpened();
    }

    void CannotOpenChest() // have to throw some error
    {
        Debug.LogError("Cannot open chest!");


    }

    #region Chest Activation

    public static void CheckForActiveChests()
    {
        Chest[] chests = FindObjectsOfType<Chest>();

        chests.Where(x => x.isActive && x != Singleton).ToList().ForEach(x => x.DeactivateChest());
    }

    void ActivateChest()
    {
        isActive = true;

        PromptSystem.SwitchPromptState(true, "interact");
    }
    void DeactivateChest()
    {
        isActive = false;

        PromptSystem.SwitchPromptState(false, "interact");
    }

    #endregion

    #region Chest Animations 
    void TriggerAnimationOpen() => animator.SetTrigger("Open");
    public void SetChestStateToOpened()
    {
        animator.SetBool("isOpened", true);
    }
    void SetOpenedState() => animator.SetBool("isOpened", haveChestBeenOpened);

    void ChestAnimationOpened() // in singleton already
    {
        SetChestStateToOpened();

        // normaly those stuff had been in the on trigger enter

        ChestOpenedNewItem();
    }

    #endregion

    void ChestOpened() // new item system I guess
    {
        haveChestBeenOpened = true;
        DeactivateChest();

        Debug.LogError("Chest opened!");

        Singleton.TriggerAnimationOpen();
    }
    void ChestOpenedNewItem()
    {
        if (shouldDeleteKeyItem && isLocked) PlayerInventory.RemoveItemFromInventory(keyItemId);

        NewItemSystem.Singleton.ShowNewItem(itemInChestId);
        PlayerInventory.AddItemToInventory(itemInChestId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || haveChestBeenOpened) return;

        Singleton = this;

        CheckForActiveChests();
        Singleton.ActivateChest();
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
