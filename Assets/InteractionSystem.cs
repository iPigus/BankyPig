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

        InteractionUI.SetActive(true);

        StartCoroutine(DisplayText(interaction.texts[0], interaction.isTextPlayer[0]));
    }

    private void Update()
    {
        if (isInteractionActive) InteractionText.text = TextToDisplay;
    }

    IEnumerator DisplayText(string text, bool isTextPlayer)
    {
        TextToDisplay = "";

        string textToDisplay = "";

        for (int i = 0; i < text.Length; i++)
        {
            textToDisplay += text[i];

            TextToDisplay = textToDisplay;

            if (i != text.Length - 1) TextToDisplay += "_";

            yield return new WaitForSeconds(timeToTypeCharacter);
        }   
    }
}
