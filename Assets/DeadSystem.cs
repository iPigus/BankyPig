using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeadSystem : MonoBehaviour
{
    public static DeadSystem Singleton { get; private set; }
    Controls controls;

    [SerializeField] GameObject DeadUI;

    float ActiveAlpha = 1f;
    float InActiveAlpha = .5f;

    [Header("UI Parts")]
    [SerializeField] TextMeshProUGUI TryAgainButton;
    [SerializeField] TextMeshProUGUI MainMenuButton;

    public static bool isDeadUIactive => Singleton.DeadUI.activeSelf;

    int _activeRow = 0;
    public int activeRow
    {
        get => _activeRow;
        set
        {
            if (!(value >= 0 && value < 2)) return;

            _activeRow = value; 
            UpdateRows();
        }
    }

    private void Awake()
    {
        Singleton = this;
        controls = new();

        if(isDeadUIactive) DeadUI.SetActive(false);

        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>().y);
    }
    
    void Move(float moveY)
    {
        if (Mathf.Abs(moveY) < .8f || !isDeadUIactive) return;

        if (moveY > 0)
        {
            activeRow--;
        }
        else
        {
            activeRow++;
        }
    }
    void UpdateRows()
    {
        int i = 0;
        SetColor(activeRow == i,ref TryAgainButton); i++;
        SetColor(activeRow == i, ref MainMenuButton); i++;
    }

    void SetColor(bool active, ref TextMeshProUGUI graphic)
    {
        if (active)
        {
            if (graphic.color.a == ActiveAlpha) return;   
            
            graphic.color = new(graphic.color.r,graphic.color.g,graphic.color.b, ActiveAlpha);
        }
        else
        {
            if (graphic.color.a == InActiveAlpha) return;  
            
            graphic.color = new(graphic.color.r,graphic.color.g,graphic.color.b, InActiveAlpha);
        }
    }

    public static void TurnOn()
    {
        Singleton.activeRow = 0;
        Singleton.StartCoroutine(TimeSlowDown());
        PromptSystem.TurnOffAllPrompts();
        Singleton.DeadUI.SetActive(true);
    }
    public static void TurnOff()
    {
        Singleton.DeadUI.SetActive(false);

    }

    static IEnumerator TimeSlowDown()
    {
        for (float i = 5; i > 0; i--)
        {
            Time.timeScale = i / 10f;

            yield return new WaitForSecondsRealtime(1f);
        }

        Time.timeScale = 0f;
    }

    #region Inputs stuff
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
