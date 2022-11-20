using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSettings : MonoBehaviour
{
    public static InGameSettings Singleton { get; private set; }

    [SerializeField] GameObject SettingsUI;


    Controls controls;
    public static bool isSettingsActive => Singleton.SettingsUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
        controls = new();

        controls.Player.Escape.performed += ctx => SwitchSettingsState();
    }

    void SwitchSettingsState()
    {


        if(isSettingsActive)
            CloseSettings();
        else 
            OpenSettings();
    }

    void OpenSettings()
    {

    }
    void CloseSettings()
    {

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
