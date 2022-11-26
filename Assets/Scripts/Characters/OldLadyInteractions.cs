using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OldLadyInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new(){ "Have you maybe seen my dog young man?", "Idk", "Hmm...",
        "Well then...", "???", "Could you help me and look for him?", "Sure thing", "Thank you young man", "If you find him I'll reward you"
    };
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, false, true, false, true, false , false};

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);

        InteractionEnded.AddListener(EventCheck);
    }

    public override Interaction GetActiveInteraction()
    {
        return Interactions[GetActiveInteractionIndex()];
    }
    public override int GetActiveInteractionIndex()
    {
        return 0;
    }

    public void EventCheck()
    {
        Debug.Log(GetActiveInteractionIndex());

        switch (GetActiveInteractionIndex())
        {
            default: break;
        }

        GetComponent<InteractableTrigger>().UpdatePlayerInteraction();
    }
}
