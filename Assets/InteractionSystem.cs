using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Singleton { get; private set; }

    [SerializeField] GameObject InteractionUI;
    [SerializeField] TextMeshProUGUI InteractionText;

    [SerializeField] GameObject CharacterName; TextMeshProUGUI CharacterNameText => CharacterName.GetComponentInChildren<TextMeshProUGUI>();
    [SerializeField] GameObject PlayerName;

    [SerializeField] RectTransform RectCharacterImage;
    [SerializeField] RectTransform RectPlayerImage;

    [Range(0f, 0.1f)] public float timeToTypeCharacter = .05f;

    public bool isTyping { get; private set; } = false;
    Coroutine TypingCoroutine { get; set; }

    Interaction activeInteraction { get; set; }
    int activeInteractionIndex { get; set; } = 0;
    string TextToDisplay { get; set; } = "";
    public bool isInteractionActive => InteractionUI.activeSelf;
    public Controls controls { get; private set; }

    private void Awake()
    {
        Singleton = this;
        controls = new();

        controls.Player.Confirm.performed += ctx => Confirm();
        controls.Player.Escape.performed += ctx => Confirm(true);

        if (InteractionUI.activeSelf) InteractionUI.SetActive(false);
    }

    private void Update()
    {
        if (isInteractionActive) InteractionText.text = TextToDisplay;
    }

    #region Loading Next Messages

    void Confirm(bool skipTypingForNextMessage = false)
    {
        if (!isInteractionActive) return;

        if (isTyping)
        {
            StopTypingAndLoadMessage();
        }
        else
        {
            LoadNextMessage(skipTypingForNextMessage);
        }
    }

    #endregion 

    #region Handling Messages
    public void LoadInteraction(Interaction interaction)
    {
        if (!interaction) { Debug.LogError("interaction is null!"); return; }
        
        if (interaction.texts.Count == 0) { Debug.LogError("interaction is doesn't contain texts!"); return; }

        RectPlayerImage.eulerAngles = new();
        RectCharacterImage.eulerAngles = new();

        Time.timeScale = 0f;

        activeInteraction = interaction;

        InteractionUI.SetActive(true);

        LoadMessage(0, interaction);
    }

    void LoadNextMessage(bool skipTyping = false) => LoadMessage(activeInteractionIndex + 1, activeInteraction, skipTyping);
    void LoadMessage(int index, Interaction interaction, bool skipTyping = false)
    {
        if (interaction.texts.Count <= index)
        {
            QuitInteraction();
            return;
        }

        activeInteractionIndex = index;

        StopAllCoroutines();

        if(!skipTyping)
            TypingCoroutine = StartCoroutine(DisplayText(interaction.texts[index], interaction.isTextPlayer[index]));
        else
        {
            TextToDisplay = activeInteraction.texts[activeInteractionIndex];

            if (interaction.isTextPlayer[index])
            {
                StartCoroutine(StartRotating(RectPlayerImage, 3f));
                StartCoroutine(StartRotating(RectCharacterImage));
            }
            else
            {
                StartCoroutine(StartRotating(RectCharacterImage, 3f));
                StartCoroutine(StartRotating(RectPlayerImage));
            }
        }
    }

    void StopTypingAndLoadMessage()
    {
        StopCoroutine(TypingCoroutine);

        isTyping = false;

        TextToDisplay = activeInteraction.texts[activeInteractionIndex];
    }

    void QuitInteraction()
    {
        Time.timeScale = 1f;

        InteractionUI.SetActive(false);
    }

    IEnumerator DisplayText(string text, bool isTextPlayer)
    {
        CharacterName.SetActive(!isTextPlayer);
        PlayerName.SetActive(isTextPlayer);

        if (isTextPlayer)
        {
            StartCoroutine(StartRotating(RectPlayerImage, 3f));
            StartCoroutine(StartRotating(RectCharacterImage));
        }
        else
        {
            StartCoroutine(StartRotating(RectCharacterImage, 3f));
            StartCoroutine(StartRotating(RectPlayerImage));
        }

        TextToDisplay = "";

        string textToDisplay = "";

        for (int i = 0; i < text.Length; i++)
        {
            isTyping = true;

            textToDisplay += text[i];

            TextToDisplay = textToDisplay;

            if (i != text.Length - 1) TextToDisplay += "_";

            yield return new WaitForSecondsRealtime(timeToTypeCharacter);
        }

        isTyping = false;
    }
    
    IEnumerator StartRotating(RectTransform objectToRotate, float RotationSpeed = 2, float MaxRotation = 5) // in anglesPerSecond
    {
        if(MaxRotation == 0 || !objectToRotate || RotationSpeed == 0) yield break;

        bool isRotatingRight = Random.Range(0, 2) == 0;

        while (true)
        {
            if (Mathf.Abs(objectToRotate.eulerAngles.z) < 30f)
            {
                if (Mathf.Abs(objectToRotate.eulerAngles.z) > Mathf.Abs(MaxRotation))
                {
                    isRotatingRight = !isRotatingRight;
                }
            }
            else
            {
                if (Mathf.Abs(360f - objectToRotate.eulerAngles.z) > Mathf.Abs(MaxRotation))
                {
                    isRotatingRight = !isRotatingRight;
                }
            }
            

            objectToRotate.eulerAngles = new(0, 0, (objectToRotate.eulerAngles.z + RotationSpeed * Time.fixedUnscaledDeltaTime * (isRotatingRight ? 1 : -1)));

            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
    #endregion

    #region Input Stuff
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
