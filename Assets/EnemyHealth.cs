using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Singleton;

    public Material WhiteMaterial;
    Material StandardMaterial;
    SpriteRenderer SpriteRenderer;
    Animator Animator;

    public int MaxHealth;

    int _health { get; set; }
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

    public bool isDead => Health <= 0;

    private void Awake()
    {
        Singleton = this;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        StandardMaterial = SpriteRenderer.material;
        _health = MaxHealth;
    }

    public void Damage(int damage, float time = 0.2f)
    {
        Health -= damage;

        StartCoroutine(WhiteFlash(time));
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
    }
    public void RemoveEnemy()
    {
        Destroy(this.gameObject);
    }
}
