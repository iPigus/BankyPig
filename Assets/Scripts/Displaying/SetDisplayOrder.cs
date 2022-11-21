using UnityEngine;
using UnityEngine.Tilemaps;

public class SetDisplayOrder : MonoBehaviour
{

    TilemapRenderer TilemapRenderer;
    SpriteRenderer SpriteRenderer;

    [SerializeField] int OffsetY;
    [SerializeField] bool isTileSet = false;

    private void Awake()
    {
        if(isTileSet)
            TilemapRenderer = GetComponent<TilemapRenderer>();
        else 
            SpriteRenderer = GetComponent<SpriteRenderer>();

        if (isTileSet)
            TilemapRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f) - OffsetY;
        else
            SpriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f) - OffsetY;
    }

    private void FixedUpdate()
    {
        if (!transform.hasChanged) return;

        if (isTileSet)
            TilemapRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f) - OffsetY;
        else
            SpriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f) - OffsetY;
    }
}
