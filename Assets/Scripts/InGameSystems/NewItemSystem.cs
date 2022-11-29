using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewItemSystem : MonoBehaviour
{
    public static NewItemSystem Singleton { get; private set; }

    Controls controls;

    [SerializeField] GameObject NewItemUI;

    [SerializeField] Image NewItemImage;
    [SerializeField] TextMeshProUGUI NewItemNameText;
    [SerializeField] TextMeshProUGUI NewItemDescribitionText;

    
    public static bool isNewItemSystemActive => Singleton.NewItemUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
        controls = new();

        controls.Player.Confirm.performed += ctx => TryToLeave();
    }

    private void Start()
    {
        StopShowingItem();
    }

                                                                         
    float timeBeforeShowingItem = 1f;
    public void ShowNewItem(int itemId) => ShowNewItem(AllItems.GetItemFromId(itemId));
    public void ShowNewItem(Item item) => ShowNewItem(item.Sprite, item.Name, item.Describtion, item.Id, item.IsEquippable);
    public void ShowNewItem(Sprite sprite, string name, string describtion, int id, bool isEquippable)
    {
        isLeaveable = false;

        NewItemUI.transform.position = PlayerMovement.Singleton.transform.position;

        NewItemImage.sprite = sprite;
        NewItemNameText.text = name;
        NewItemDescribitionText.text = describtion;

        timeBeforeShowingItem = Time.timeScale;
        Time.timeScale = 0f;

        NewItemUI.SetActive(true);
    }

    public void StopShowingItem()
    {
        Time.timeScale = timeBeforeShowingItem;

        NewItemUI.SetActive(false);
    }
    bool _isLeaveable = true;
    bool isLeaveable
    {
        get => _isLeaveable;
        set
        {
            if(!value) _isLeaveable = false;

            StartCoroutine(LeavingCooldown());
        }
    }
    IEnumerator LeavingCooldown(float time = .3f)
    {
        if(time <= 0) yield return null;

        yield return new WaitForSecondsRealtime(time);

        _isLeaveable = true;
    }
    void TryToLeave()
    {
        if (!NewItemUI.activeSelf) return; 

        if (!isLeaveable) return;
        
        StopShowingItem();
    }



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
