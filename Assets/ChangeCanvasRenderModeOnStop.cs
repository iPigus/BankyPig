using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvasRenderModeOnStop : MonoBehaviour
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void Update()
    {
        if (!PlayerManager.isInAnySystem)
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            if(canvas.worldCamera == null) canvas.worldCamera = FindObjectOfType<Camera>();
            canvas.sortingLayerName = "UI";
        }
    }
}
