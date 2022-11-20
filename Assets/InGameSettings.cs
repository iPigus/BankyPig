using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameSettings : MonoBehaviour
{
    public static InGameSettings Singleton { get; private set; }

    [SerializeField] GameObject SettingsUI;
    [SerializeField] GameObject PauseUI;

    [Header("PauseParts")]
    [SerializeField] TextMeshProUGUI ResumePause;
    [SerializeField] TextMeshProUGUI SettingsPause;
    [SerializeField] TextMeshProUGUI ResetScenePause;

    [Header("SettingsParts")]
    [SerializeField] TextMeshProUGUI BackSettings;
    [SerializeField] TextMeshProUGUI PostProcessingSettings;
    [SerializeField] TextMeshProUGUI FullScreenSettings;
    [SerializeField] TextMeshProUGUI AddWeaponSettings;

    Color ActiveColor = new(1, 1, 1, 1);
    Color DeactiveColor = new(1, 1, 1, .5f);

    int _pauseActiveRow = 0;
    int pauseActiveRow
    {
        get => _pauseActiveRow;
        set
        {
            if (!(value >= 0 && value < 3)) return; 
                
            _pauseActiveRow = value;
            SetActiveColorInPause();
        }
    }

    int _settingsActiveRow = 0;
    int settingsActiveRow
    {
        get => _settingsActiveRow;
        set
        {
            if (!(value >= 0 && value < 4)) return;

            _settingsActiveRow = value;
            SetActiveColorInSettings();
        }
    }

    Controls controls;
    public static bool isSettingsActive => Singleton.SettingsUI.activeSelf;
    public static bool isPauseActive => Singleton.PauseUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
        controls = new();

        controls.Player.Escape.performed += ctx => SwitchPauseState();
        controls.Player.Confirm.performed += ctx => Confirm();
        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>().y);
    }
    void Move(float Ymove)
    {
        if (Ymove == 0) return;

        if(Ymove > 0)
        {
            if (isPauseActive) pauseActiveRow--;
            if (isSettingsActive) settingsActiveRow--;
        }
        else
        {
            if (isPauseActive) pauseActiveRow++;
            if (isSettingsActive) settingsActiveRow++;
        }
    }
    void Confirm()
    {
        if (isPauseActive)
        {
            switch (pauseActiveRow)
            {
                case 0: ClosePause(true); break;
                case 1: ClosePause(); OpenSettings(); break;
                case 2: UnityEngine.SceneManagement.SceneManager.LoadScene(0); break;
                default: break;
            }
        }
        else if (isSettingsActive)
        {
            switch (settingsActiveRow)
            {
                case 0: CloseSettings(); OpenPause(); break;
                default: break;
            }
        }
    }
    void SwitchPauseState()
    {
        if (PlayerManager.isInAnySystem) return;

        if (isPauseActive)
            ClosePause(true);
        else if (isSettingsActive)
            CloseSettings();
        else
            OpenPause();
    }
    void OpenPause()
    {
        Time.timeScale = 0f;
        SetActiveColorInPause();

        PauseUI.SetActive(true);
    }
    void ClosePause(bool returnTimescale = false)
    {
        PauseUI.SetActive(false);

        if(returnTimescale) Time.timeScale = 1.0f;
    }

    void OpenSettings()
    {
        SetActiveColorInSettings();

        SettingsUI.SetActive(true);
    }
    void CloseSettings()                        
    {
        SettingsUI.SetActive(false);
    }
    void SetActiveColorInPause()
    {
        int i = 0;
        ResumePause.color = pauseActiveRow == i ? ActiveColor : DeactiveColor; ; i++;
        SettingsPause.color = pauseActiveRow == i ? ActiveColor : DeactiveColor; ; i++;
        ResetScenePause.color = pauseActiveRow == i ? ActiveColor : DeactiveColor; ; i++;
    }
    void SetActiveColorInSettings()
    {
        int i = 0;
        BackSettings.color = settingsActiveRow == i ? ActiveColor : DeactiveColor; i++;
        PostProcessingSettings.color = settingsActiveRow == i ? ActiveColor : DeactiveColor; i++;
        FullScreenSettings.color = settingsActiveRow == i ? ActiveColor : DeactiveColor; i++;
        AddWeaponSettings.color = settingsActiveRow == i ? ActiveColor : DeactiveColor; i++;
    }

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
