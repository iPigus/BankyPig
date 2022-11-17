using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Singleton { get; private set; }

    [SerializeField] GameObject InventoryUI;

    [SerializeField] TextMeshProUGUI NameText; 
    [SerializeField] TextMeshProUGUI DescribtionText;
    [SerializeField] GameObject EquitableObject;

    [SerializeField] GameObject basicItem;
    [SerializeField] GameObject ItemListParent;
    
    List<GameObject> items = new();

    public bool isInventoryOpen => InventoryUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
    }

    public void OpenInventory()
    {
        LoadInventory();

        InventoryUI.SetActive(true);
    }
    public void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }

    void LoadInventory()
    {
        items = new();

        for (int i = 0; i < ItemListParent.transform.childCount; i++)
        {
            Destroy(ItemListParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < PlayerInventory.Singleton.Items.Count; i++)
        {
            AddItem(PlayerInventory.Singleton.Items[i]);
        }
    }

    public void AddItem(Item item)
    {
        GameObject newItem = Instantiate(basicItem, ItemListParent.transform);  
        
        newItem.AddComponent<ItemStats>().SetItemStats(item.Sprite, item.Name,item.Describtion, item.Id);
        
        items.Add(newItem);
    }
}
