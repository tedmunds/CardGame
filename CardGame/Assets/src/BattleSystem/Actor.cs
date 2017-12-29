using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public const int DEFAULT_ENERGY_REGEN = 2;

    public int health       { get; private set; }
    public int maxHealth    { get; private set; }

    public int energy       { get; private set; }
    public int maxEnergy    { get; private set; }

    public int energyRegen  { get; private set; }
    
    public int id           { get; private set; }
    public BattleTeam team  { get; private set; }

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

        health = 100;
        energy = 20;
        energyRegen = DEFAULT_ENERGY_REGEN;

        maxHealth = health;
        maxEnergy = energy;
    }
	

    public void OnTurnStart()
    {
        currentTurn = new Turn(id);
        energy = Mathf.Min(energy + energyRegen, maxEnergy);
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
        health = Mathf.Max(health - damageAmount, 0);

        if(health == 0)
        {
            // TODO: died
        }
    }

}
