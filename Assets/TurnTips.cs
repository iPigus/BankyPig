using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTips : MonoBehaviour
{
    static TurnTips Singleton;
    
    public static void EndTourTip()
    {
        if (!Singleton)
        {
            Debug.LogError("Couldn't find Sinleton!"); return;
        }

        Singleton.transform.GetChild(0).gameObject.SetActive(true);    
    }

    private void OnDisable()
    {
        Singleton.transform.GetChild(0).gameObject.SetActive(false);
    }
}
