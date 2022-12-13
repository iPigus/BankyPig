using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySystem : MonoBehaviour
{
    public GameObject Butterfly;

    [Header("Butterfly Spawning")]
    [Range(0.1f, 10f)] public float ButterflyFrequency = 1f;
    [Range(0, 2000)] public uint MaxButterflyCount = 40;
    [Range(0, 2000)] public uint SpawnButterflyCount = 15;
    [Range(0f, 100f)] public float ButterflySpawnRange = 35f;
    [Range(0f, 10000f)] public float DestroyButterflyTime = 1000f;

    [Header("Butterfly Size")]
    [Range(0f, 1f)] public float ButterflyMinSize = .4f;
    [Range(0f, 1f)] public float ButterflyMaxSize = .6f;

    uint Tick { get; set; }  // TickRate => 50

    List<GameObject> Butterflies = new();

    private void Awake()
    {
        for (int i = 0; i < SpawnButterflyCount; i++)
        {
            SpawnButterfly(FindPositionToSpawnButterfly(true));
        }
    }

    private void FixedUpdate()
    {
        Tick++;

        if (Tick % (2000 / ButterflyFrequency) == 0)
        {
            SpawnButterfly(FindPositionToSpawnButterfly());
        }
    }

    void SpawnButterfly(Vector2 position)
    {
        GameObject spawnedButterfly = Instantiate(Butterfly, position, Quaternion.identity, transform);

        float ButterflySize = Random.Range(ButterflyMinSize, ButterflyMaxSize);

        spawnedButterfly.transform.localScale = new(ButterflySize, ButterflySize);

        Butterflies.Add(spawnedButterfly);
        StartCoroutine(DestroyCloud(spawnedButterfly));
    }

    Vector2 FindPositionToSpawnButterfly(bool canSpawnOnScreen = false)
    {
        Vector2 value = new();

        if (canSpawnOnScreen)
        {
            for (int i = 0; i < 1000; i++)
            {
                value = GetRandomFloat(-ButterflySpawnRange, ButterflySpawnRange);

                if (isFarEnoughFromOtherClouds(value)) break;
            }
        }
        else
        {
            for (int i = 0; i < 1000; i++)
            {
                value = GetRandomFloat(-ButterflySpawnRange, ButterflySpawnRange);

                if (isFarEnoughFromOtherClouds(value) && !isCloudOnScreen(value)) break;
            }
        }

        return value;
    }

    Vector2 GetRandomFloat(float min, float max)
    {
        /*
        System.Random rand = new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup * 1000));

        float Range = max - min;

        Vector2 value = new(Range * (float)rand.NextDouble() * (float)rand.NextDouble() - (Range / 2f), 0f);

        rand = new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup * 500));

        value = new(value.x, Range * (float)rand.NextDouble() * (float)rand.NextDouble() - (Range / 2f));
        */

        Vector2 value = new(Random.Range(min, max), Random.Range(min, max));

        return value;
    }
    Camera _camera;
    Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = FindObjectOfType<Camera>();
            }

            return _camera;
        }
    }

    float screenToWorldSizeRatioX => 13f / 5.4f * Camera.orthographicSize;
    float screenToWorldSizeRatioY => 6.9f / 5.4f * Camera.orthographicSize;

    float clostestCloudDistance = 1f;

    bool isCloudOnScreen(Vector2 positon)
    {
        if (Mathf.Abs(positon.x) < screenToWorldSizeRatioX) return true;
        if (Mathf.Abs(positon.y) < screenToWorldSizeRatioY) return true;

        return false;
    }
    bool isFarEnoughFromOtherClouds(Vector2 position)
    {
        if (Butterflies.Count == 0) return true;
        bool value = true;

        for (int i = 0; i < Butterflies.Count; i++)
        {
            if ((position - (Vector2)Butterflies[i].transform.position).magnitude < clostestCloudDistance) return false;
        }

        return value;
    }

    IEnumerator DestroyCloud(GameObject cloud)
    {
        yield return new WaitForSeconds(DestroyButterflyTime);

        while (isCloudOnScreen(cloud.transform.position))
        {
            yield return new WaitForSeconds(1f);
        }

        Butterflies.Remove(cloud);
        Destroy(cloud);
    }
}
