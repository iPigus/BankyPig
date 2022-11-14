using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Singleton;

    public Material WhiteMaterial;
    Material StandardMaterial;
    SpriteRenderer SpriteRenderer;

    public int MaxHealth;

    int health;
    public int Health
    {
        get => health;
        set
        {
            if (value < 0) value = 0;

            if (value > MaxHealth) value = MaxHealth;

            health = value;
        }
    }

    public bool isDead => Health <= 0;

    private void Awake()
    {
        Singleton = this;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        StandardMaterial = SpriteRenderer.material;
        health = MaxHealth;
    }

    public void Damage(int damage, float time = 0.2f)
    {
        health -= damage;

        StartCoroutine(WhiteFlash(time));
    }
    IEnumerator WhiteFlash(float time)
    {
        SpriteRenderer.material = WhiteMaterial;

        yield return new WaitForSeconds(time);

        SpriteRenderer.material = StandardMaterial;
    }
}
