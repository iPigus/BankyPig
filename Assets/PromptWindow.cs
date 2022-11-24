using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptWindow : MonoBehaviour
{
    [Header("Base Data")]
    [SerializeField] int keyBaseId = 0;

    [Header("Other Stuff")]
    [SerializeField] Image BackgroundImage;
    [SerializeField] Image KeyImage;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] List<Sprite> keysKeyboard = new List<Sprite>();
    [SerializeField] List<Sprite> keysSwitch = new List<Sprite>();
    
    public void SetPrompt(string text, int letterId = 0)
    {
        this.text.text = text;



        if (keysKeyboard.Count - 1 > letterId) Debug.LogError("NO KEY SPRITE ATTACHED WITH INDEX: " + letterId);
        else
            KeyImage.sprite = keysKeyboard[letterId];
    }
}
