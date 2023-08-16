using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovePointText : MonoBehaviour
{
    private void Awake()
    {
        if (transform.parent.parent.name.ToLower().Contains("player")) TurnBasedManager.playerTurnPoints = GetComponent<TextMeshProUGUI>();
        else TurnBasedManager.enemyTurnPoints = GetComponent<TextMeshProUGUI>();
    }
}
