using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Singleton;

    public Material WhiteMaterial;
    Material StandardMaterial;
    SpriteRenderer SpriteRenderer;
    Animator Animator;

    public int MaxHealth;

    int _health;
    public int Health
    {
        get => _health;
        set
        {
            if (value < 0) value = 0;

            if (value > MaxHealth) value = MaxHealth;

            _health = value;

            if (_health == 0) Death();
        }
    }

    public static bool isDead => Singleton.Health <= 0;

    private void Awake()
    {
        Singleton = this;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        StandardMaterial = SpriteRenderer.material;
        _health = MaxHealth;
    }

    public static void Damage(int damage, float time = 0.2f)
    {
        if (isDead) return; 

        Singleton.Health -= damage;

        Singleton.StartCoroutine(Singleton.WhiteFlash(time));
    }
    IEnumerator WhiteFlash(float time)
    {
        SpriteRenderer.material = WhiteMaterial;

        yield return new WaitForSeconds(time);

        SpriteRenderer.material = StandardMaterial;
    }
    public void Death()
    {
        if (Animator.GetBool("isDead")) return;

        Animator.SetBool("isDead", true);
        StartCoroutine(Dead());
    }
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);

        Animator.SetBool("Dead", true);
    }
}
