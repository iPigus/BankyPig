using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;

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
    int itemCount => items.Count;

    int _activeItem = 0;
    public int activeItem
    {
        get => _activeItem;
        set
        {
            if (value < 0 || value >= itemCount) return;

            _activeItem = value;
            SetItemsAcive();
        }
    }

    public bool isInventoryOpen => InventoryUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        LoadInventory();
        SetItemsAcive(true);
    }

    public void OpenInventory()
    {
        SetItemsAcive(true);

        InventoryUI.SetActive(true);
    }
    public void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
    void LoadInventory()
    {
        items = new();

        ItemListParent.GetComponent<RectTransform>().anchoredPosition = new(0, 0);

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
        
        newItem.AddComponent<ItemStats>().SetItemStats(item.Sprite, item.Name,item.Describtion, item.Id, item.IsEquippable);

        newItem.GetComponent<RectTransform>().anchoredPosition = new(itemCount * 480, 0);
        
        items.Add(newItem);
    }

    bool _movementInventoryCooldown = false;
    bool movementInventoryCooldown 
    {
        get => _movementInventoryCooldown;
        set
        {
            if (value)
            {
                _movementInventoryCooldown = true;

                StartCoroutine(MovementCooldown());
            }
        }
    }
    IEnumerator MovementCooldown(float time = .3f)
    {
        yield return new WaitForSecondsRealtime(time);

        _movementInventoryCooldown = false;
    }

    public void MovementInInventory(float inputX)
    {
        if (isMoving)
        {
            if(Waiting != null) StopCoroutine(Waiting);

            Waiting = StartCoroutine(WaitToMove(inputX)); return;
        }

        if ((inputX < -.3f && activeItem == 0) || (inputX > .3f && activeItem + 1 >= itemCount)) return; 

        if (inputX > .3f)
            activeItem++;
        else if (inputX < -.3f)
            activeItem--;
    }

    Coroutine Waiting = null;
    IEnumerator WaitToMove(float inputX)
    {
        while (isMoving)
        {
            yield return new WaitForSecondsRealtime(.01f);
        }

        MovementInInventory(inputX); yield break;
    }

    void SetItemsAcive(bool moveInstantly = false)
    {
        if (!moveInstantly)
        {
            if (MovingCoroutine == null) MovingCoroutine = StartCoroutine(MoveToActiveItem());
            if (ScaleCoroutine == null) ScaleCoroutine = StartCoroutine(ScaleItems());
        }
        else
        {
            ItemListParent.GetComponent<RectTransform>().anchoredPosition = new(-activeItem * 480, 0);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].GetComponent<RectTransform>().localScale = Vector2.one * (i == activeItem ? 1f : .6f);
            }
        }

        NameText.text = PlayerInventory.Singleton.Items[activeItem].Name;
        DescribtionText.text = PlayerInventory.Singleton.Items[activeItem].Describtion;
        EquitableObject.SetActive(PlayerInventory.Singleton.Items[activeItem].IsEquippable);
    }

    bool isMoving => MovingCoroutine != null || ScaleCoroutine != null;

    Coroutine MovingCoroutine = null;
    Coroutine ScaleCoroutine = null;
    IEnumerator MoveToActiveItem()
    {
        float difference = ItemListParent.GetComponent<RectTransform>().anchoredPosition.x - (-activeItem * 480);

        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSecondsRealtime(.01f);

            ItemListParent.GetComponent<RectTransform>().anchoredPosition =
                new(ItemListParent.GetComponent<RectTransform>().anchoredPosition.x - difference * 0.05f, 0);
        }

        MovingCoroutine = null;
    }
    IEnumerator ScaleItems()
    {
        for (float i = 1; i <= 20; i++)
        {
            yield return new WaitForSecondsRealtime(.01f);

            for (int j = 0; j < itemCount; j++)
            {
                if (j == activeItem)
                {
                    items[j].GetComponent<RectTransform>().localScale = Vector2.one * (.6f + i / (20 * 2.5f));
                }
                else
                {
                    if (items[j].GetComponent<RectTransform>().localScale.magnitude > .36f)
                    items[j].GetComponent<RectTransform>().localScale = Vector2.one * (1f - i / (20 * 2.5f));
                }
            }
        }
        ScaleCoroutine = null;
    }
}
