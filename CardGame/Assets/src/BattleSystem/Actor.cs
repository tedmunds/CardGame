using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public const int DEFAULT_ENERGY_REGEN = 2;

    public delegate void StartTurnDelegate(Turn currentTurn);
    public delegate void ActorDeathDelegate(Actor killed);

    public int health       { get; private set; }
    public int maxHealth    { get; private set; }

    public int energy       { get; private set; }
    public int maxEnergy    { get; private set; }

    public int energyRegen  { get; private set; }
    
    public int id           { get; private set; }
    public BattleTeam team  { get; private set; }

    public bool isDead      { get; private set; }

    private int turnsAlive;
    
    public StartTurnDelegate onStartTurn;
    public ActorDeathDelegate onDeath;

    // Turn that is currently beingn written before being dispatched
    private Turn currentTurn;
    public Turn GetCurrentTurn()
    {
        return currentTurn;
    }

    public void Init(int id, BattleTeam team)
    {
        this.id = id;
        this.team = team;

        isDead = false;
        turnsAlive = 0;

        health = 100;
        energy = 20;
        energyRegen = DEFAULT_ENERGY_REGEN;

        maxHealth = health;
        maxEnergy = energy;
    }
	

    public void OnTurnStart()
    {
        if(isDead)
        {
            return;
        }

        currentTurn = new Turn(id, turnsAlive);
        energy = Mathf.Min(energy + energyRegen, maxEnergy);

        if(onStartTurn != null)
        {
            onStartTurn.Invoke(currentTurn);
        }

        turnsAlive += 1;
    }


    public float ConsumeEnergy(int amount)
    {
        int newEnergy = (int)Mathf.Max(energy - amount, 0);
        int amountConsumed = energy - newEnergy;

        energy = newEnergy;

        return amountConsumed;
    }

    public void RecieveDamage(int damageAmount)
    {
        if(isDead)
        {
            return;
        }

        health = Mathf.Max(health - damageAmount, 0);

        if(health == 0)
        {
            if(onDeath != null)
            {
                isDead = true;
                onDeath.Invoke(this);
            }
        }
    }

    public void Heal(int healAmount)
    {
        if(isDead)
        {
            return;
        }

        health = Mathf.Min(health + healAmount, maxHealth);
    }

}
