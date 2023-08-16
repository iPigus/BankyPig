using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMoveUI : MonoBehaviour
{
    [Header("Basic Info")]
    public int turnPoints = 1;
    public bool isReusable = false; 

    [Header("Attacking")]
    public bool isAttack = false;
    public Vector2Int Attack = new();

    [Header("Shielding")]
    public bool isShield = false;
    public Vector2Int Shield = new();

    [Header("Healing")]
    public bool isHeal = false;
    public Vector2Int Heal = new();

    private void Awake()
    {
        
    }

    public void Click()
    {
        if (!TurnBasedManager.canMakeMove(turnPoints)) return;
        if (!isAttack && !isShield && !isHeal) { Debug.LogError("It ain't doing nothing"); return; }
        System.Random random = new();

        if (isAttack)
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if (amount > 0) TurnBasedManager.PlayerAttack(amount);
        }
        if (isShield) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);
            
            if (amount != 0) TurnBasedManager.PlayerShield(amount);
        }
        if (isHeal) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if (amount != 0) TurnBasedManager.PlayerHeal(amount);
        }

        TurnBasedManager.UseTurnPoints(turnPoints);
    }

    public void Hide()
    {

    }
    public void Disable()
    {

    }
    public void Enable()
    {

    }
}
