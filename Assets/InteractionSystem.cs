using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Singleton { get; private set; }

    [SerializeField] GameObject InteractionUI;
    [SerializeField] TextMeshProUGUI InteractionText;

    string TextToDisplay { get; set; } = "";

    [Range(0f, 0.1f)] public float timeToTypeCharacter = .05f;

    public bool isInteractionActive => InteractionUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
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
        for (int i = 0; i < text.Length; i++)
        {
            TextToDisplay.Insert(i, text[i].ToString());
            yield return new WaitForSeconds(timeToTypeCharacter);
        }   
    }
}
