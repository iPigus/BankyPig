using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    Collider2D Collider;

    private void Awake()
    {
        CheckIfColliderIsTrigger();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    #region Checking 
    void CheckIfColliderIsTrigger()
    {
#if UNITY_EDITOR
        if (!Collider) Collider = GetComponents<Collider2D>().Where(x => x.isTrigger).First();
        else
        if (!Collider.isTrigger) Collider = GetComponents<Collider2D>().Where(x => x.isTrigger).First();
#endif
    }
    #endregion
}
