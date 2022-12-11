using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Displaying Settings")]
    public Color SelectedColor = Color.yellow;
    public Color DeselectedColor = Color.white;
    public float SelectedFontSize = 48;
    public float DeselectedFontSize = 42f;

    [Header("Activation Parts")]

    [SerializeField] GameObject SettingsManager;
    [SerializeField] GameObject MultiplayerManager;
    [SerializeField] GameObject SettingsCanvas;
    [SerializeField] GameObject MultiplayerCanvas;
    [SerializeField] GameObject MenuCanvas;

    [Header("Menu Parts")]
    [SerializeField] TextMeshProUGUI EnterHubText;
    [SerializeField] TextMeshProUGUI MultiplayerText;
    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] TextMeshProUGUI SettingsText;

    int _SelectedRow = 0;
    int SelectedRow
    {
        get => _SelectedRow;
        set
        {
            if (!(value >= 0 && value < 4) || value == _SelectedRow) return;

            _SelectedRow = value;
            UpdateActiveRows();
        }
    }

    Controls controls;

    private void Awake()
    {
        controls = new();

        Time.timeScale = 1;
    }
    private void Start()
    {
        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>().y);
        controls.Player.Confirm.performed += ctx => Select();
    }

    void Move(float movementY)
    {
        if (Mathf.Abs(movementY) < .5f) return;

        if (movementY < 0) SelectedRow++;
        else SelectedRow--;
    }
    void Select()
    {
        switch (SelectedRow)
        {
            case 0: EnterHub(); break;
            case 1: EnterMultiplayer(); break;
            case 2: EnterTutorial(); break;
            case 3: EnterSettings(); break;
        }
    }

    void UpdateActiveRows()
    {
        if (SelectedRow == 0) Highlight(EnterHubText); else LowLight(EnterHubText);
        if (SelectedRow == 1) Highlight(MultiplayerText); else LowLight(MultiplayerText);
        if (SelectedRow == 2) Highlight(TutorialText); else LowLight(TutorialText);
        if (SelectedRow == 3) Highlight(SettingsText); else LowLight(SettingsText);
    }

    #region Enter Functions

    public void EnterHub()
    {
        StartCoroutine(LoadScene(1));
    }
    public void EnterMultiplayer()
    {
        MultiplayerCanvas.SetActive(true);
        MultiplayerManager.SetActive(true);
        MenuCanvas.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void EnterTutorial()
    {
        StartCoroutine(LoadScene(4));
    }

    IEnumerator LoadScene(int scene)
    {
        //SceneTransition.PlayLeavingAnimation();
        //yield return new WaitForSecondsRealtime(1f);

        yield return null; Debug.Log("ANIMATION TO PLACE IN HERE!");
        SceneManager.LoadScene(scene);
    }
    public void EnterSettings()
    {
        SettingsManager.SetActive(true);
        SettingsCanvas.SetActive(true);
        MenuCanvas.SetActive(false);
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
