using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySystem : MonoBehaviour
{
    public static ButterflySystem Singleton { get; private set; }

    public int ButterfliesToSpawn = 200;
    public float SpawnRange = 50f;
    public float MinimumSpawnDistance = .5f;

    [SerializeField] GameObject Butterfly;

    List<GameObject> ButterflyList = new();



    private void Awake()
    {
        Singleton = this;
    }
}
