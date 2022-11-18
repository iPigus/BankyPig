using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NewItemSystem : MonoBehaviour
{
    public static NewItemSystem Singleton { get; private set; }

    [SerializeField] GameObject NewItemUI;

    [SerializeField] Image NewItemImage;
    [SerializeField] TextMeshProUGUI NewItemNameText;
    [SerializeField] TextMeshProUGUI NewItemDescribitionText;


    
    public bool isNewItemSystemActive => NewItemUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        StopShowingItem();
    }

    public void ShowNewItem(int itemId) => ShowNewItem(AllItems.GetItemFromId(itemId));
    public void ShowNewItem(Item item) => ShowNewItem(item.Sprite, item.Name, item.Describtion, item.Id, item.IsEquippable);
    public void ShowNewItem(Sprite sprite, string name, string describtion, int id, bool isEquippable)
    {
        NewItemUI.transform.position = PlayerMovement.Singleton.transform.position;

        NewItemImage.sprite = sprite;
        NewItemNameText.text = name;
        NewItemDescribitionText.text = describtion;


        NewItemUI.SetActive(true);
    }

    public void StopShowingItem()
    {
        NewItemUI.SetActive(false);
    }
}
