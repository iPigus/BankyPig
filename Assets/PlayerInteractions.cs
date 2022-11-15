using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public bool isPlayerInteractable = true;
    public bool isInterationAvailable => interactionToChose;
    public bool isInInteraction => InteractionSystem.Singleton.isInteractionActive;
    public Interaction interactionToChose { get; set; } 

    public Controls Controls { get; set; }

    private void Awake()
    {
        Controls = new();

        Controls.Player.Interact.performed += ctx => InteractPerformed();
    }
    private void Update()
    {
        
    }
    void InteractPerformed()
    {
        if (!isInterationAvailable) return;

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
