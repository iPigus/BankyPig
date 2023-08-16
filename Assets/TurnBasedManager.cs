using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnBasedManager : MonoBehaviour
{
    public static TurnBasedManager Singleton;

    public List<Statlider> playerSliders = new();    
    public List<Statlider> enemySliders = new();

    TurnPlayer player; TurnEnemy enemy;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        Statlider playerHealth = playerSliders.Where(x => x.name.ToLower().Contains("health")).First(); playerSliders.Remove(playerHealth);
        Statlider playerEnergy = playerSliders.First(); playerSliders.Remove(playerEnergy); if (playerSliders.Count() > 1) Debug.LogError("Too many player sliders!");

        Statlider enemyHealth = enemySliders.Where(x => x.name.ToLower().Contains("health")).First(); enemySliders.Remove(enemyHealth);
        Statlider enemyEnergy = enemySliders.First(); enemySliders.Remove(playerEnergy); if (enemySliders.Count() > 1) Debug.LogError("Too many player sliders!");

        player = new(0, 5, 5, 5, 5, 2, playerHealth, playerEnergy);
        enemy = new(0, 7, 7, 4, 4, 2, enemyHealth, enemyEnergy);
    }

    public static void PlayerAttack(int damage) => Singleton.enemy.TakeDamage(damage);
    public static void PlayerShield(int shield) => Singleton.player.AddShield(shield);
    public static void PlayerHeal(int heal) => Singleton.player.AddHealth(heal);
    public static void UseTurnPoints(int turnPoints) => Singleton.player.UseMovePoints(turnPoints);

    public static bool canMakeMove(int turnPoints)
    {
        if (!Singleton) { Debug.LogError("No Singleton!"); return false; }
        if (Singleton.player == null) { Debug.LogError("Player is null!"); return false; }

        return Singleton.player.movePoints >= turnPoints;
    }
}

class TurnPlayer
{
    public TurnPlayer(int shield, int maxHealth, int health, int maxEnergy, int energy, int movePoints, Statlider healthSlider, Statlider energySlider)
    {
        this.shield = shield;
        this.maxHealth = maxHealth;
        this.health = health;
        this.maxEnergy = maxEnergy;
        this.energy = energy;
        this.movePoints = movePoints;
        this.healthSlider = healthSlider;
        this.energySlider = energySlider;

        UpdateSliders();
    }
    Statlider healthSlider; Statlider energySlider;
    public int shield { get; private set; } = 0;
    public int maxHealth { get; private set; }  
    public int health { get; private set; }
    public int maxEnergy { get; private set; }
    public int energy { get; private set; }
    public int movePoints { get; private set; }

    void UpdateSliders()
    {
        healthSlider.Set(health, maxHealth);
        energySlider.Set(energy, maxEnergy);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateSliders();
    }
    public void AddHealth(int heal)
    {
        health += heal;
        UpdateSliders();
    }
    public void AddShield(int shield)
    {
        this.shield += shield;
        UpdateSliders();
    }
    public void UseMovePoints(int movePoints) => this.movePoints -= movePoints;
}

class TurnEnemy
{
    public TurnEnemy(int shield, int maxHealth, int health, int maxEnergy, int energy, int movePoints, Statlider healthSlider, Statlider energySlider)
    {
        this.shield = shield;
        this.maxHealth = maxHealth;
        this.health = health;
        this.maxEnergy = maxEnergy;
        this.energy = energy;
        this.movePoints = movePoints;
        this.healthSlider = healthSlider;
        this.energySlider = energySlider;

        UpdateSliders();
    }

    Statlider healthSlider; Statlider energySlider;
    public int shield { get; protected set; } = 0;
    public int maxHealth { get; protected set; }
    public int health { get; protected set; }
    public int maxEnergy { get; protected set; }
    public int energy { get; protected set; }

    public int movePoints { get; protected set; }
    void UpdateSliders()
    {
        healthSlider.Set(health, maxHealth);
        energySlider.Set(energy, maxEnergy);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateSliders();
    }
    public void UseMovePoints(int movePoints) => this.movePoints -= movePoints;
}

//class TurnBoss : TurnEnemy
//{
//    
//}