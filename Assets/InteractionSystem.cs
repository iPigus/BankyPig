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

    string TextToDisplay { get; set; } = "";

    [Range(0f, 0.1f)] public float timeToTypeCharacter = .05f;

    public bool isInteractionActive => InteractionUI.activeSelf;

    private void Awake()
    {
        Singleton = this;

        if (InteractionUI.activeSelf) InteractionUI.SetActive(false);
    }

    public void LoadInteraction(Interaction interaction)
    {
        if (!interaction) { Debug.LogError("interaction is null!"); return; }

        Time.timeScale = 0f;

        InteractionUI.SetActive(true);

        StopAllCoroutines();

        StartCoroutine(DisplayText(interaction.texts[0], interaction.isTextPlayer[0]));
    }

    private void Update()
    {
        if (isInteractionActive) InteractionText.text = TextToDisplay;
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
            textToDisplay += text[i];

            TextToDisplay = textToDisplay;

            if (i != text.Length - 1) TextToDisplay += "_";

            yield return new WaitForSecondsRealtime(timeToTypeCharacter);
        }   
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
    
}
