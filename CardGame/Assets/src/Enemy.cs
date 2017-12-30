using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public string enemyName = "Default Enemy";

    // The actor this enemy controls
    private Actor actor;

    // Called when the enemy is first created
    public void OnSpawn(Actor actor)
    {
        this.actor = actor;
        actor.onStartTurn += OnTurnStart;
    }

    /// <summary>
    /// Called at the start of this enemies turn
    /// </summary>
    protected virtual void OnTurnStart(Turn currentTurn)
    {
        // TEMP: alternate between attacking player and healing
        if(currentTurn.turnNum % 2 == 0)
        {
            Battle.instance.GetPlayer().RecieveDamage(10);
        }
        else
        {
            actor.Heal(10);
        }
    }

	
	protected virtual void Update() 
	{
		
	}
}
