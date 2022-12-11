using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Displaying Settings")]
    public Color SelectedColor = Color.yellow;
    public Color DeselectedColor = Color.white;
    public float SelectedFontSize = 48;
    public float DeselectedFontSize = 42f;

    [Header("Menu Activation")]
    [SerializeField] MenuManager MenuManager;
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject SettingsCanvas;


    [Header("Settings Parts")]
    [SerializeField] TextMeshProUGUI FullscreenText;
    [SerializeField] TextMeshProUGUI ResolutionText;
    [SerializeField] TextMeshProUGUI ActualResolutionText;
    [SerializeField] TextMeshProUGUI MusicText;
    [SerializeField] TextMeshProUGUI SoundsText;
    [SerializeField] TextMeshProUGUI BackText;

    [SerializeField] Graphic MusicSliderFill;
    [SerializeField] Graphic MusicSliderHandle;
    [SerializeField] Graphic SoundsSliderFill;
    [SerializeField] Graphic SoundsSliderHandle;

    Slider MusicSlider;
    Slider SoundsSlider;


    int _SelectedRow = 0;
    int SelectedRow
    {
        get => _SelectedRow;
        set
        {
            if (!(value >= -1 && value < 4) || value == _SelectedRow) return;

            _SelectedRow = value;
            UpdateActiveRows();
        }
    }

    Controls controls;

    private void Awake()
    {
        controls = new();
    }
    private void Start()
    {
        MusicSlider = MusicText.GetComponentInChildren<Slider>();
        SoundsSlider = SoundsText.GetComponentInChildren<Slider>();

        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>().y);
        controls.Player.Movement.performed += ctx => SliderPerform(ctx.ReadValue<Vector2>().x);
        controls.Player.Movement.performed += ctx => ResChange(ctx.ReadValue<Vector2>().x);
        controls.Player.Movement.canceled += ctx => SliderPerform(0);
        controls.Player.Confirm.performed += ctx => Select();
        controls.Player.Escape.performed += ctx => Back();

        LoadValues();
        UpdateSliderTextValues();
        GetResolutions();
    }

    #region Handling Stuff

    void ResChange(float move)
    {
        if (SelectedRow != 1 || Mathf.Abs(move) < .5f) return;

        if (move > 0) ResolutionUp();
        else ResolutionDown();
    }

    void Move(float movementY)
    {
        if (Mathf.Abs(movementY) < .5f) return;

        if (movementY < 0) SelectedRow++;
        else SelectedRow--;
    }

    void LoadValues()
    {
        FullscreenText.text = $"Fullscreen: {(Screen.fullScreen ? "On" : "Off")}";
        ActualResolutionText.text = $"{Screen.width}x{Screen.height}";
    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            MoveSlider();
        }
        UpdateSliderTextValues();
    }

    void UpdateSliderTextValues()
    {
        MusicText.text = "Music : " + Mathf.Round(100 * MusicSlider.value / (MusicSlider.maxValue - MusicSlider.minValue)).ToString();
        SoundsText.text = "Sounds : " + Mathf.Round(100 * SoundsSlider.value / (SoundsSlider.maxValue - SoundsSlider.minValue)).ToString();
    }

    void SliderPerform(float move)
    {
        if (!sliderRows.Contains(SelectedRow) || Mathf.Abs(move) < .5f) { isSliding = false; return; }

        isSliding = true;
        movementX = move;
    }

    bool isSliding = false;
    float movementX;
    List<int> sliderRows = new() { 2, 3, 4 };
    void MoveSlider()
    {
        if (!sliderRows.Contains(SelectedRow)) return;

        switch (SelectedRow)
        {
            case 2: MusicSlider.value += Time.deltaTime * Mathf.Round(movementX) * .7f; break;
            case 3: SoundsSlider.value += Time.deltaTime * Mathf.Round(movementX) * .7f; break;
            default: break;
        }
    }
    void Select()
    {
        switch (SelectedRow)
        {
            case -1: Back(); break;
            case 0: ChangeFullscreen(); break;
        }
    }

    void UpdateActiveRows()
    {
        if (SelectedRow == -1) Highlight(BackText); else LowLight(BackText);
        if (SelectedRow == 0) Highlight(FullscreenText); else LowLight(FullscreenText);
        if (SelectedRow == 1) Highlight(ResolutionText); else LowLight(ResolutionText);
        if (SelectedRow == 1) Highlight(ActualResolutionText); else LowLight(ActualResolutionText);

        if (SelectedRow == 2) Highlight(MusicText); else LowLight(MusicText);
        if (SelectedRow == 2) Highlight(MusicSliderFill); else LowLight(MusicSliderFill);
        if (SelectedRow == 2) Highlight(MusicSliderHandle); else LowLight(MusicSliderHandle);

        if (SelectedRow == 3) Highlight(SoundsText); else LowLight(SoundsText);
        if (SelectedRow == 3) Highlight(SoundsSliderHandle); else LowLight(SoundsSliderHandle);
        if (SelectedRow == 3) Highlight(SoundsSliderFill); else LowLight(SoundsSliderFill);
    }

    #endregion

    #region Enter Functions

    public void ChangeFullscreen()
    {
        FullscreenText.text = $"Fullscreen: {(Screen.fullScreen ? "Off" : "On")}";

        Screen.fullScreen = !Screen.fullScreen;
    }

    List<Vector2> resoltionsVectorized = new();

    int activeResolutionIndex;

    void GetResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Vector2 current = new(resolutions[i].width, resolutions[i].height);

            if (resoltionsVectorized.Contains(current)) continue;

            if (current.x / current.y > 1.8f || current.x / current.y <= 1f) continue;

            resoltionsVectorized.Add(current);
        }

        for (int i = 0; i < resoltionsVectorized.Count; i++)
        {
            if (!(Screen.width == resoltionsVectorized[i].x && Screen.height == resoltionsVectorized[i].y)) continue;

            activeResolutionIndex = i;
        }
    }
    public void ResolutionUp()
    {
        if (activeResolutionIndex == resoltionsVectorized.Count - 1) return;

        activeResolutionIndex++;
        Screen.SetResolution(Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].x), Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].y), Screen.fullScreen);
        ActualResolutionText.text = $"{Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].x)}x{Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].y)}";
    }
    public void ResolutionDown()
    {
        if (activeResolutionIndex == 0) return;

        activeResolutionIndex--;
        Screen.SetResolution(Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].x), Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].y), Screen.fullScreen);
        ActualResolutionText.text = $"{Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].x)}x{Mathf.RoundToInt(resoltionsVectorized[activeResolutionIndex].y)}";
    }

    public void Back()
    {
        MenuManager.gameObject.SetActive(true);
        MenuCanvas.gameObject.SetActive(true);
        SettingsCanvas.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    #endregion

    #region Transitions between Selected and Deselected

    List<TextMeshProUGUI> inHighlight = new();
    List<TextMeshProUGUI> inLowlight = new();

    void Highlight(TextMeshProUGUI text)
    {
        if (text.color == SelectedColor && text.fontSize == SelectedFontSize) return;
        if (inHighlight.Contains(text)) return;

        inHighlight.Add(text);

        if (inLowlight.Contains(text)) inLowlight.Remove(text);

        StartCoroutine(ColorAndFontTransition(text, SelectedColor, SelectedFontSize));
    }

    void LowLight(TextMeshProUGUI text)
    {
        if (text.color == DeselectedColor && text.fontSize == DeselectedFontSize) return;
        if (inLowlight.Contains(text)) return;

        inLowlight.Add(text);

        if (inHighlight.Contains(text)) inHighlight.Remove(text);

        StartCoroutine(ColorAndFontTransition(text, DeselectedColor, DeselectedFontSize));
    }

    IEnumerator ColorAndFontTransition(TextMeshProUGUI text, Color colorToTransition, float fontSizeToTransition)
    {
        if (text.color == colorToTransition && text.fontSize == fontSizeToTransition) yield break;

        Color startingColor = text.color;
        float startingFontSize = text.fontSize;

        for (float i = 1; i <= 10; i++)
        {
            yield return new WaitForSecondsRealtime(.02f);

            text.color = startingColor * ((10 - i) / 10) + colorToTransition * i / 10;
            text.fontSize = startingFontSize * ((10 - i) / 10) + fontSizeToTransition * i / 10;
        }

        if (inHighlight.Contains(text)) inHighlight.Remove(text);
        else if (inLowlight.Contains(text)) inLowlight.Remove(text);
    }

    List<Graphic> inHighColor = new();
    List<Graphic> inLowColor = new();

    void Highlight(Graphic graphic)
    {
        if (graphic.color == SelectedColor) return;
        if (inHighColor.Contains(graphic)) return;

        inHighColor.Add(graphic);

        if (inLowColor.Contains(graphic)) inLowColor.Remove(graphic);

        StartCoroutine(ColorTransition(graphic, SelectedColor, SelectedFontSize));
    }

    void LowLight(Graphic graphic)
    {
        if (graphic.color == DeselectedColor) return;
        if (inLowColor.Contains(graphic)) return;

        inLowColor.Add(graphic);

        if (inHighColor.Contains(graphic)) inHighColor.Remove(graphic);

        StartCoroutine(ColorTransition(graphic, DeselectedColor, DeselectedFontSize));
    }

    IEnumerator ColorTransition(Graphic graphic, Color colorToTransition, float fontSizeToTransition)
    {
        if (graphic.color == colorToTransition) yield break;

        Color startingColor = graphic.color;

        for (float i = 1; i <= 10; i++)
        {
            yield return new WaitForSecondsRealtime(.02f);

            graphic.color = startingColor * ((10 - i) / 10) + colorToTransition * i / 10;
        }

        if (inHighColor.Contains(graphic)) inHighColor.Remove(graphic);
        else if (inLowColor.Contains(graphic)) inLowColor.Remove(graphic);
    }

    #endregion

    #region Input stuff 

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    #endregion 
}
