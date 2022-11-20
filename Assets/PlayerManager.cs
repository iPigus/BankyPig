using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool isInAnySystem => PlayerInventory.isInventoryOpen || PlayerMovement.Singleton.isAttacking || NewItemSystem.isNewItemSystemActive || InGameSettings.isInGameSettings;
}
