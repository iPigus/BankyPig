using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] int ItemId;

    Collider2D Collider;

    private void Awake()
    {
        CheckIfColliderIsTrigger();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (PlayerInventory.doesInventoryContainItem(ItemId)) return;

        PlayerInventory.ShowPickable(gameObject, ItemId);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;  
        
        PlayerInventory.HidePickable(gameObject, ItemId);
    }

    #region Checking 
    void CheckIfColliderIsTrigger()
    {
#if UNITY_EDITOR
        if (!Collider) Collider = GetComponents<Collider2D>().Where(x => x.isTrigger).First();
        else
        if (!Collider.isTrigger) Collider = GetComponents<Collider2D>().Where(x => x.isTrigger).First();
#endif
    }
    #endregion
}
