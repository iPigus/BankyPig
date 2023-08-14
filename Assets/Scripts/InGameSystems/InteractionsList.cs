using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractionsList : MonoBehaviour
{
    public abstract List<Interaction> Interactions { get; set; }

    public abstract Interaction GetActiveInteraction();
    public abstract int GetActiveInteractionIndex();

    public abstract UnityEvent InteractionEnded { get; set; }

    public virtual void AddInteraction(List<string> interactionTexts, List<bool> isPlayerText)
    {
        Interactions.Add(new(interactionTexts, isPlayerText));
    }
}
