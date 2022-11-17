using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions Singleton { get; private set; }

    public bool isPlayerInteractable = true;
    public bool isInterationAvailable => interactionToChose;
    public bool isInInteraction => InteractionSystem.Singleton.isInteractionActive;
    public Interaction interactionToChose { get; set; } 

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
        if (!isInterationAvailable || isInInteraction || PlayerInventory.Singleton.isInventoryOpen || PlayerMovement.Singleton.isAttacking) return;

        InteractionSystem.Singleton.LoadInteraction(interactionToChose);
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
