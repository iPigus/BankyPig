using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Singleton { get; private set; }

    [SerializeField] GameObject InteractionUI;
    [SerializeField] TextMeshProUGUI InteractionText;

    [SerializeField] GameObject CharacterName; TextMeshProUGUI CharacterNameText => CharacterName.GetComponentInChildren<TextMeshProUGUI>();
    [SerializeField] GameObject PlayerName;

    [SerializeField] RectTransform RectCharacterImage; Image characterImage => RectCharacterImage.GetComponentInChildren<Image>();
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
    public void LoadInteraction(Interaction interaction, GameObject interactionCharacter, Sprite characterSprite)
    {
        if (!interaction) { Debug.LogError("interaction is null!"); return; }
        
        if (interaction.texts.Count == 0) { Debug.LogError("interaction is doesn't contain texts!"); return; }

        characterImage.sprite = characterSprite;

        CharacterNameText.text = interactionCharacter.name;

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

        StopAllCoroutines();

        activeInteractionIndex = index;

        ChangeColorsToActive(interaction.isTextPlayer[activeInteractionIndex]);

        if (!skipTyping)
            TypingCoroutine = StartCoroutine(DisplayText(interaction.texts[activeInteractionIndex], interaction.isTextPlayer[activeInteractionIndex]));
        else
        {
            TextToDisplay = activeInteraction.texts[activeInteractionIndex];

            if (interaction.isTextPlayer[activeInteractionIndex])
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

        PlayerInteractions.Singleton.InvokeInteractionEndEvent();

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
                    isRotatingRight = false;
                }
            }
            else
            {
                if (Mathf.Abs(360f - objectToRotate.eulerAngles.z) > Mathf.Abs(MaxRotation))
                {
                    isRotatingRight = true;
                }
            }
            

            objectToRotate.eulerAngles = new(0, 0, (objectToRotate.eulerAngles.z + RotationSpeed * Time.fixedUnscaledDeltaTime * (isRotatingRight ? 1 : -1)));

            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    void ChangeColorsToActive(bool activePlayer = true)
    {
        StartCoroutine(WhiteOut(activePlayer));
        StartCoroutine(BlackOut(activePlayer));
        StartCoroutine(ChangeSizes(activePlayer));
    }

    Image PlayerImage => RectPlayerImage.GetComponentInChildren<Image>();
    Image CharacterImage => RectCharacterImage.GetComponentInChildren<Image>();

    IEnumerator WhiteOut(bool isTextPlayer = true)
    {
        Image ImageToChange = isTextPlayer ? PlayerImage : CharacterImage;

        if (ImageToChange.color != Color.white)
        {
            float startingTint = ImageToChange.color.g;

            for (float i = 1; i <= 20; i++)
            {
                float tint = startingTint + (((1f - startingTint) / 20f) * i);

                ImageToChange.color = new(tint, tint, tint, 1);

                yield return new WaitForSecondsRealtime(.01f);
            }
        }

        yield break;
    }
    IEnumerator BlackOut(bool isTextPlayer = true)
    {
        Image ImageToChange = isTextPlayer ? CharacterImage : PlayerImage;

        if (ImageToChange.color != new Color(.5f,.5f,.5f,1f))
        {
            float startingTint = ImageToChange.color.g;

            for (float i = 1; i <= 20; i++)
            {
                float tint = startingTint - (((startingTint - 0.5f) / 20f) * i);

                ImageToChange.color = new(tint, tint, tint, 1);

                yield return new WaitForSecondsRealtime(.01f);
            }
        }

        yield break;
    }

    IEnumerator ChangeSizes(bool isTextPlayer = true)
    {
        RectTransform ImageToScaleUp = isTextPlayer ? RectPlayerImage : RectCharacterImage;
        RectTransform ImageToShrinkDown = isTextPlayer ? RectCharacterImage : RectPlayerImage;

        float startScaleUpSize = ImageToScaleUp.localScale.x;
        float startShrinkDownSize = ImageToShrinkDown.localScale.x;

        for (int i = 1; i <= 20; i++)
        {
            float scaleUpScale = startScaleUpSize + (((1f - startScaleUpSize) / 20f) * i);
            float shrinkDownScale = startShrinkDownSize - (((startShrinkDownSize - 0.9f) / 20f) * i);

            if (ImageToScaleUp.localScale.magnitude != 1f) ImageToScaleUp.localScale = Vector3.one * scaleUpScale;
            if (ImageToShrinkDown.localScale.magnitude != .9f) ImageToShrinkDown.localScale = Vector3.one * shrinkDownScale;

            yield return new WaitForSecondsRealtime(.01f);
        }

        yield break;
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
