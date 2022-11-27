using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaldGuyInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new() { "Mmm...         ", "???", "Mmm...         ", "What are you doing?", "Mmm...         ", "weirdo"};
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, true, false, true };

    List<string> interactionTexts1 = new() { "Mmm...         ", "Who are you dude?", "I'm...         a monk bro", "sure...", "by the way have you maybe seen my father?", "I dont think so", "Mmm...         " , "ok." };
    List<bool> isPlayerText1 = new List<bool>() { false, true, false, true, false, true , false,true };

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);
        AddInteraction(interactionTexts1, isPlayerText1);

        InteractionEnded.AddListener(EventCheck);
    }

    bool interaction0ended = false; 

    public override Interaction GetActiveInteraction()
    {
        return Interactions[GetActiveInteractionIndex()];
    }
    public override int GetActiveInteractionIndex()
    {
        if (interaction0ended) return 1;

        return 0;
    }

    public void EventCheck()
    {
        Debug.Log(GetActiveInteractionIndex());

        switch (GetActiveInteractionIndex())
        {
            case 0: interaction0ended = true; break;
            default: break;
        }

        GetComponent<InteractableTrigger>().UpdatePlayerInteraction();
    }
}
