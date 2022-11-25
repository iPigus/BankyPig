using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PromptSystem : MonoBehaviour
{
    public static PromptSystem Singleton { get; private set; }

    public static int InputType = 0;

    [SerializeField] List<GameObject> Prompts = new List<GameObject>();

    [SerializeField] GameObject basePrompt;

    GameObject InteractionPrompt { get; set; }
    GameObject SwitchPrompt { get; set; }

    Dictionary<string, GameObject> PromptDictionary { get; set; } = new();

    private void Awake()
    {
        Singleton = this;

        if (Prompts.Count >= 1)
        {
            InteractionPrompt = Prompts[0];
            PromptDictionary.Add("interact", InteractionPrompt);
            InteractionPrompt.SetActive(false);
        }
        if (Prompts.Count >= 2)
        {
            SwitchPrompt = Prompts[1];
            PromptDictionary.Add("switch", SwitchPrompt);
            SwitchPrompt.SetActive(false);
        }
    }

    public static void SwitchPromptState(bool active, string promptName) => SwitchPromptState(active, promptName, 0);
    public static void SwitchPromptState(bool active,string promptName, int keyId)
    {
        if(Singleton.PromptDictionary.TryGetValue(promptName.ToLower(), out GameObject prompt))
        {
            if(prompt.activeSelf != active) prompt.SetActive(active);             
        }
        else // need to make it add new prompt instead
        {
            Debug.LogError("Couldn't find prompt with name :" + promptName);

            GameObject newPrompt = Instantiate(Singleton.basePrompt, Singleton.transform.GetChild(0));

            newPrompt.GetComponent<PromptWindow>().SetPrompt(promptName, keyId);

            Singleton.PromptDictionary.Add(promptName.ToLower(), newPrompt);

            newPrompt.SetActive(active);
        }
    }
}
