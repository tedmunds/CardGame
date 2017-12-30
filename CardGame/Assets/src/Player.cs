using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField]
    public Deck deckPrototype;

    private Deck activeDeck;
    private Actor playerActor;

    public Deck GetDeck() { return activeDeck; }

	public void BattleStart(Actor playerActor) 
	{
        this.playerActor = playerActor;
        activeDeck = Instantiate(deckPrototype);
        activeDeck.Init(playerActor);

        playerActor.onDeath += PlayerDied;
    }
	
	protected virtual void Update() 
	{
		
	}


    public void PlayerDied(Actor killed)
    {
        Battle.instance.EndBattle(BattleEndState.Death);
    }
}
