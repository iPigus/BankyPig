using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    Interaction interaction => interactions.GetActiveInteraction();

    InteractionsList interactions;

    public bool isInteractable { get; private set; } = true;

    private void Awake()
    {
        CheckIfHasTrigger();

        interactions = GetComponent<InteractionsList>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.TryGetComponent(out PlayerInteractions playerInteractions))
        {
            playerInteractions.interactionToChose = interaction;
            playerInteractions.activeInteractionList = interactions;
            PromptSystem.SwitchPromptState(true, "Interaction");
        }
        else
        {
            Debug.LogError("Player doesn't have PlayerInteractions!");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.TryGetComponent(out PlayerInteractions playerInteractions))
        {
            if (playerInteractions.interactionToChose == interaction && !PlayerInteractions.isInInteraction)
            {
                playerInteractions.interactionToChose = null;
                playerInteractions.activeInteractionList = null;
                PromptSystem.TurnPromptOff();
            }
        }
        else
        {
            Debug.LogError("Player doesn't have PlayerInteractions!");
        }
    }

    void CheckIfHasTrigger()
    {
#if UNITY_EDITOR

        Collider2D[] colliders = GetComponents<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].isTrigger) return;
        }

        Debug.LogError("InteractableTrigger in " + gameObject.name + " doesn't have any Colliders with isTrigger == true");

#endif
    }

}

public class Interaction
{
    public Interaction(List<string> Texts, List<bool> IsTextPlayer)
    {
        if (Texts.Count != IsTextPlayer.Count)
        {
            if (Texts.Count > IsTextPlayer.Count) Debug.LogError("Not enough IsTextPlayer assigned! Difference: " + (-IsTextPlayer.Count + Texts.Count));
            else 
            if (Texts.Count < IsTextPlayer.Count) Debug.LogError("Not enough Texts assigned! Difference: " + (IsTextPlayer.Count - Texts.Count));
        }

        this.texts = Texts;
        this.isTextPlayer = IsTextPlayer;
    }

    public static implicit operator bool(Interaction exists)
    {
        return exists != null;
    }

    public List<string> texts { set; get; }
    public List<bool> isTextPlayer { set; get; }

}