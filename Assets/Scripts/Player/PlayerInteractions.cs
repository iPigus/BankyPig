using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions Singleton { get; private set; }

    public bool isPlayerInteractable = true;
    public bool isInterationAvailable => interactionToChose;
    public static bool isInInteraction => InteractionSystem.Singleton.isInteractionActive;
    public Interaction interactionToChose { get; set; }
    public GameObject interactionCharacter { get; set; }
    public Sprite interactionCharacterSprite { get; set; }
    public InteractionsList activeInteractionList { get; set; }

    public Controls Controls { get; set; }

    private void Awake()
    {
        Singleton = this;
        Controls = new();

        Controls.Player.Interact.performed += ctx => InteractPerformed();
    }
    private void Update()
    {
        
    }
    void InteractPerformed()
    {
        if (!isInterationAvailable || isInInteraction || PlayerManager.isInAnySystem) return;

        InteractionSystem.Singleton.LoadInteraction(interactionToChose, interactionCharacter, interactionCharacterSprite);
    }

    public void InvokeInteractionEndEvent()
    {
        if (!activeInteractionList) return;

        activeInteractionList.InteractionEnded.Invoke();
    }

    private void OnEnable()
    {
        Controls.Enable();
    }
    private void OnDisable()
    {
        Controls.Disable();
    }
}
