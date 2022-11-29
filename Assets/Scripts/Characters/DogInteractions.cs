using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DogInteractions : InteractionsList
{
    public override List<Interaction> Interactions { get; set; } = new();
    public override UnityEvent InteractionEnded { get; set; } = new();

    List<string> interactionTexts0 = new() { "*barks*", "???", "*barking intensifies*", "   ...weird dog" };
    List<bool> isPlayerText0 = new List<bool>() { false, true, false, true };

    List<string> interactionTexts1 = new() { "*barks*", "what the hell does this dog wants?", "*more dog barks*", "hmm...       ", "maybe I need to bring him something" };
    List<bool> isPlayerText1 = new List<bool>() { false, true, false, true, true };

    List<string> interactionTexts2 = new() { "*barks*", "hmm maybe I can give him the bone" , "*happily barking*", "lol" };
    List<bool> isPlayerText2 = new List<bool>() { false, true, false, true };

    private void Awake()
    {
        AddInteraction(interactionTexts0, isPlayerText0);
        AddInteraction(interactionTexts1, isPlayerText1);
        AddInteraction(interactionTexts2, isPlayerText2);

        InteractionEnded.AddListener(EventCheck);
    }

    bool isEvent0Invoked = false;
    bool isEvent1Invoked = false;
    bool isEvent2Invoked = false;

    public override Interaction GetActiveInteraction()
    {
        return Interactions[GetActiveInteractionIndex()];
    }
    public override int GetActiveInteractionIndex()
    {
        if (PlayerInventory.doesInventoryContainItem(2)) return 2;

        if (isEvent0Invoked) return 1;

        return 0;
    }

    public void EventCheck()
    {
        Debug.Log(GetActiveInteractionIndex());

        switch (GetActiveInteractionIndex())
        {
            case 0: isEvent0Invoked = true; break;
            case 1: isEvent1Invoked = true; break;
            case 2: isEvent2Invoked = true; EventItemSystem.InvokeEvent("PickUpDog0"); break;
            default: break;
        }

        GetComponent<InteractableTrigger>().UpdatePlayerInteraction();
    }
}
