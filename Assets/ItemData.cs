using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData _singleton;
    public static ItemData Singleton
    {
        get => _singleton;

        set
        {
            if(_singleton == null ) _singleton = value;
            else 
            Destroy(value.gameObject);
        }
    }

    public static List<Sprite> ItemSprites => Singleton.itemSprites;
    public static List<string> ItemNames => Singleton.itemNames;
    public static List<string> ItemDescribtions => Singleton.itemDescribtions;


    [SerializeField] List<Sprite> itemSprites = new();
    [SerializeField] List<string> itemNames = new();
    [SerializeField] List<string> itemDescribtions = new();

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }
}
