using System.Collections;
using System.Collections.Generic;
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

            UpdateActiveChest();
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
    public bool haveChestBeenOpened = false;

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

    }
    void ChestOpened() // new item system I guess
    {

    }

    static void UpdateActiveChest()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Singleton != null || !collision.CompareTag("Player") || Singleton.haveChestBeenOpened) return;

        Singleton = this;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Singleton.Equals(this) || !collision.CompareTag("Player")) return;

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
