using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMoveUI : MonoBehaviour
{
    public int turnPoints = 1;

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

    private void OnMouseUpAsButton()
    {
        Click();
    }

    void Click()
    {
        if (!TurnBasedManager.canMakeMove(turnPoints)) return;
        System.Random random = new();

        if (isAttack)
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if (amount > 0)
            {
                TurnBasedManager.PlayerAttack()
            }
        }
        if (isShield) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);
            
            if (amount != 0)
            {

            }
        }
        if (isHeal) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if(amount != 0)
            {

            }
        }
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
