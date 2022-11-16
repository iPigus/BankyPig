using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] GameObject Cloud;

    [Header ("Cloud Spawning")]
    [Range(0.1f, 10f)] public float CloudFrequency = 1f;
    [Range(0, 400)] public uint MaxCloudCount = 40;
    [Range(0, 400)] public uint SpawnCloudCount = 15;

    [Header("Cloud Movement")]
    [Range(0f, 360f)] public float CloudMoveDirection = 0f;
    [Range(0f, 0.3f)] public float CloudMoveMaxSpeed = 0.1f;
    [Range(0f, 0.3f)] public float CloudMoveMinSpeed = 0.09f;

    uint Tick { get; set; }  // TickRate => 50

    List<GameObject> Clouds = new();

    private void Awake()
    {
        for (int i = 0; i < SpawnCloudCount; i++)
        {
            SpawnCloud(FindPositionToSpawnCloud(true));
        }  
    }

    private void FixedUpdate()
    {
        Tick++;

        if(Tick % (2000/CloudFrequency) == 0 )
        {
            SpawnCloud(FindPositionToSpawnCloud());
        }
    }

    void SpawnCloud(Vector2 position)
    {
        GameObject spawnedCloud = Instantiate(Cloud, position, Quaternion.identity, transform);

        Clouds.Add(spawnedCloud);
        spawnedCloud.GetComponent<Rigidbody2D>().velocity = Random.Range(CloudMoveMinSpeed, CloudMoveMaxSpeed) * new Vector2(Mathf.Cos(CloudMoveDirection / 180f * Mathf.PI), Mathf.Sin(CloudMoveDirection / 180f * Mathf.PI));
        StartCoroutine(DestroyCloud(spawnedCloud));
    }

    Vector2 FindPositionToSpawnCloud(bool canSpawnOnScreen = false)
    {
        Vector2 value = new();

        if (canSpawnOnScreen)
        {
            for (int i = 0; i < 1000; i++)
            {
                value = GetRandomFloat(-35f, 35f);

                if (isFarEnoughFromOtherClouds(value)) break;
            }
        }
        else
        {
            for (int i = 0; i < 1000; i++)
            {
                value = GetRandomFloat(-35f, 35f);

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

        Vector2 value = new(Random.Range(min, max),Random.Range(min, max));

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

    float clostestCloudDistance = 7f;

    bool isCloudOnScreen(Vector2 positon)
    {
        if (Mathf.Abs(positon.x) < screenToWorldSizeRatioX) return true;
        if (Mathf.Abs(positon.y) < screenToWorldSizeRatioY) return true;

        return false;
    }
    bool isFarEnoughFromOtherClouds(Vector2 position)
    {
        if(Clouds.Count == 0) return true;
        bool value = true;

        for (int i = 0; i < Clouds.Count; i++)
        {
            if ((position - (Vector2)Clouds[i].transform.position).magnitude < 7f) return false;
        }

        return value;
    }
    
    IEnumerator DestroyCloud(GameObject cloud)
    {
        float DestroyCloudTime = 240f;

        yield return new WaitForSeconds(DestroyCloudTime);

        Clouds.Remove(cloud);
        Destroy(cloud);
    }
}