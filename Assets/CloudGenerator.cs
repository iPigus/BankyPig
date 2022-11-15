using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] GameObject Cloud;

    [Range(0,10)] public float CloudFrequency;

    uint Tick { get; set; }

    List<GameObject> Clouds = new();

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        Tick++;

        if(Tick % (1000/CloudFrequency) == 0 )
        {
            SpawnCloud(FindPositionToSpawnCloud());
        }
    }

    void SpawnCloud(Vector2 position)
    {
        Clouds.Add(Instantiate(Cloud, position, Quaternion.identity, transform));

        StartCoroutine(DestroyCloud(Clouds[Clouds.Count - 1]));
    }

    Vector2 FindPositionToSpawnCloud()
    {
        Vector2 value = new();

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
