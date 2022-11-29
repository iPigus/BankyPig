using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OldLadyInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new(){ "Have you maybe seen my dog young man?", "Idk", "Hmm...",
        "Well then...", "???", "Could you help me and look for him?", "Sure thing", "Thank you young man", "I'll give you reward young man if you can bring him back"
    };
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, false, true, false, true, false , false};

    List<string> interactionTexts1 = new(){ "Have you find my dog young man?", "Not yet", "Hmm...        I'll go look for him then"
    };
    List<bool> isPlayerText1 = new List<bool>() { false, true, false};

    List<string> interactionTexts2 = new(){ "Have you find my dog young man?", "Not yet", "Hmm...        I'll go look for him then"
    };
    List<bool> isPlayerText2 = new List<bool>() { false, true, false };

    List<string> interactionTexts3 = new(){ "Have you find my dog young man?", "Not yet", "Hmm...        I'll go look for him then"
    };
    List<bool> isPlayerText3 = new List<bool>() { false, true, false };

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);
        AddInteraction(interactionTexts1, isPlayerText1);
        AddInteraction(interactionTexts2, isPlayerText2);
        AddInteraction(interactionTexts3, isPlayerText3);

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
