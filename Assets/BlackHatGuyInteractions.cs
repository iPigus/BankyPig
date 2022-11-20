using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlackHatGuyInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new (){ "What are you looking for in here?", "???", "Who you are?",
        "Me.", "Okay...", "Anyway you wanna help me find a key to this house?", "Sure.", "I think key maybe somewhere in this forest, but Idk tbh"
    };
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, true, false, false, true, false };

    List<string> interactionTexts1 = new(){ "Oh you have found this key, nice", "Can you show me it to me a while?", "???", "Bro I wanna open these door...", "...And?", "And I'll let you in if it works", "Fine then..."
    };
    List<bool> isPlayerText1 = new List<bool>() { false, false, true, false, true, false, true };

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);
        AddInteraction(interactionTexts1, isPlayerText1);

        InteractionEnded.AddListener(EventCheck);
    }
    void AddInteraction(List<string> interactionTexts, List<bool> isPlayerText)
    {
        Interactions.Add(new(interactionTexts, isPlayerText));
    }

    public override Interaction GetActiveInteraction()
    {
        return Interactions[GetActiveInteractionIndex()];
    }

    public override int GetActiveInteractionIndex()
    {
        if (PlayerInventory.doesInventoryContainItem(1))
            return 1;
        else
            return 0;
    }

    public void EventCheck()
    {
        if (GetActiveInteractionIndex() == 0) return;

        switch (GetActiveInteractionIndex())
        {
            case 1:  EventItemSystem.InvokeEvent("DoorOpening0"); break;
            default: break;
        }
    }
}