using UnityEngine;

public class SetDisplayOrder : MonoBehaviour
{
    SpriteRenderer SpriteRenderer;

    [SerializeField] int OffsetY;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!transform.hasChanged) return;

        SpriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f) - OffsetY;
    }
}
