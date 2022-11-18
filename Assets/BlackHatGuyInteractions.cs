using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHatGuyInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();

    List<string> interactionTexts0 = new (){ "What are you looking for in here?", "???", "Who you are?",
        "Me.", "Okay...", "Anyway you wanna help me find a key to this house?", "Sure.", "I think key maybe somewhere in this forest, but Idk tbh"
    };
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, true, false, false, true, false };

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);


    }
    void AddInteraction(List<string> interactionTexts, List<bool> isPlayerText)
    {
        Interactions.Add(new(interactionTexts, isPlayerText));
    }
}
