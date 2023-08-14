using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{
    static Door _Singleton = null;
    public static Door Singleton
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

        controls.Player.Interact.performed += ctx => TryOpenDoors();

#if UNITY_EDITOR

        if (GetComponent<Animator>() == null) Debug.LogError("DOORS DOESN'T HAVE ANIMATOR ATTACHED!"); 

#endif
    }

    public bool isLocked = false;
    public int keyItemId = 0;
    public bool shouldDeleteKeyItem = true;
    public bool haveDoorsBeenOpened = false;
    public bool isActive { get; set; } = false;

    Animator animator { get; set; }

    void TryOpenDoors()
    {
        if (PlayerManager.isInAnySystem || Singleton == null) return;

        if (Singleton.isLocked)
        {
            if (PlayerInventory.doesInventoryContainItem(Singleton.keyItemId)) Singleton.DoorsOpened();
            else
                Singleton.CannotOpenDoors();
        }
        else Singleton.DoorsOpened();
    }

    void CannotOpenDoors() // have to throw some error
    {
        Debug.LogError("Cannot open doors!");


    }

    #region Door Activation

    public static void CheckForActiveChests()
    {
        Door[] chests = FindObjectsOfType<Door>();

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
    public void SetDoorsStateToOpened()
    {
        animator.SetBool("isOpened", true);
    }
    void SetOpenedState() => animator.SetBool("isOpened", haveDoorsBeenOpened);

    void ChestAnimationOpened() // in singleton already
    {
        SetDoorsStateToOpened();

        // normaly those stuff had been in the on trigger enter

        DoorsOpenedNewItem();
    }

    #endregion

    void DoorsOpened() // new item system I guess
    {
        haveDoorsBeenOpened = true;
        DeactivateChest();

        Debug.LogError("Doors opened!");

        Singleton.TriggerAnimationOpen();
    }
    void DoorsOpenedNewItem()
    {
        if (shouldDeleteKeyItem && isLocked) PlayerInventory.RemoveItemFromInventory(keyItemId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || haveDoorsBeenOpened) return;

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
