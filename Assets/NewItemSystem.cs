using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemSystem : MonoBehaviour
{
    public static NewItemSystem Singleton { get; private set; }

    [SerializeField] GameObject NewItemUI;

    

    public bool isNewItemSystemActive => NewItemUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
    }
}
