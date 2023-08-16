using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnMoveUI : MonoBehaviour
{
    [Header("Basic Info")]
    public string moveName = string.Empty;
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

    [Header("Serielized Fields")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI reusableText;
    [SerializeField] TextMeshProUGUI turnPointsText;

    [SerializeField] GameObject AttackStat;
    [SerializeField] GameObject HealStat;
    [SerializeField] GameObject ShieldStat;

    private void Awake()
    {
        InfoUpdate();
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


    void InfoUpdate()
    {
        
    }
}
