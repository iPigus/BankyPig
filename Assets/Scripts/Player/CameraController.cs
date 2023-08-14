using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public static CameraController Singleton { get; private set; }

    new Camera camera;

    bool doPostEffects => camera.GetUniversalAdditionalCameraData().renderPostProcessing;
    private void Awake()
    {
        Singleton = this;

        camera = GetComponent<Camera>();
        
        camera.GetUniversalAdditionalCameraData().renderPostProcessing = PlayerPrefs.GetInt("GFX") == 0;
    }

    public static bool postEffects => Singleton.doPostEffects;
    public static void ChangePostProcessing() => ChangePostProcessing(!Singleton.doPostEffects);
    public static void ChangePostProcessing(bool toState)
    {
        PlayerPrefs.SetInt("GFX", toState ? 0 : 1);

        Singleton.camera.GetUniversalAdditionalCameraData().renderPostProcessing = toState;
    }

}
