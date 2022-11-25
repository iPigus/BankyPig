using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Range(1, 100)] public int Damage = 1;
    [SerializeField] bool isTrigger = true;

    void EnterZone(GameObject collision, bool IsTrigger)
    {
        if (!collision.CompareTag("Player") || IsTrigger != isTrigger) return;

        PlayerHealth.Damage(Damage);
    }
    void ExitZone(GameObject collision, bool IsTrigger)
    {
        if (!collision.CompareTag("Player") || IsTrigger != isTrigger) return;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterZone(collision.gameObject, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitZone(collision.gameObject, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnterZone(collision.gameObject, false);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        ExitZone(collision.gameObject, false);
    }
}
