using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Singleton { get; private set; }

    Controls controls;

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject PressAnyKeyText;

    bool isMenuLoaded = false;

    private void Awake()
    {
        Singleton = this;
        controls = new();

        Time.timeScale = 1.0f;

        controls.Player.Confirm.performed += ctx => Confirm();
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isMenuLoaded)
        {
            LoadMenu();
        }
    }

    void Confirm()
    {
        if (!isMenuLoaded && SceneManager.GetActiveScene().buildIndex != 0) return;

        LoadGame();
    }

    void LoadMenu()
    {
        PressAnyKeyText.SetActive(false);
        MainMenu.SetActive(true);

        StartCoroutine(ZoomingIn());
    }

    Vector2 positionToZoomIn => new(-6.6f, 25f);
    float cameraSizeToZoomIn => 12f;

    IEnumerator ZoomingIn()
    {
        float startingCameraZoom = Camera.main.orthographicSize;
        Vector2 startingPosition = Camera.main.transform.position;

        const float framesToCut = 20;

        Vector3 move = startingPosition - positionToZoomIn;
        float cameraZoomChange = startingCameraZoom - cameraSizeToZoomIn;

        for (int i = 0; i <= framesToCut; i++)
        {
            float x = (i + 1) / framesToCut;

            Camera.main.orthographicSize -= cameraZoomChange / framesToCut * Mathf.Sqrt(Mathf.Sin(x));
            Camera.main.transform.position -= move / framesToCut * Mathf.Sqrt(Mathf.Cos(x));

            yield return new WaitForFixedUpdate();
        }

        isMenuLoaded = true;
    }

    void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
