using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SamuraiInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new(){ "It's dangerous to go alone...", "???", "What do you want from me?",
        "???", "Okay...", "What are you doing here?", "Idk", "I'm just some npc", "okay...      sooo?", "I can give you some quest I guess",
        "okay", "bring me please the ancient sword of my ancestors", "sure", "where is it btw?", "idk", "???","that's the point of this quest bro", "ok"
    };
    List<bool> isPlayerText0 = new List<bool>() { false,true,false,
        true,false,true,false,false,true,false,
        true,false,true,true,false,true,false,true};

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
