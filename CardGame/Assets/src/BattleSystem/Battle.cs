using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void CardEffect(Actor activator, Actor target, Card source);

public enum BattleTeam
{
    Player,
    Opponent
}


[RequireComponent(typeof(Player))]
public class Battle : MonoBehaviour
{
    public static Battle instance { get; private set; }

    [SerializeField]
    private GameObject actorPrototype = null;
    
    private ActorSlot[] actorSlots;

    // All players in the battle, can be AI or PC
    private List<Actor> actors = new List<Actor>();

    private Player player;
    
	public void Start()
    {
        instance = this;
        actorSlots = GameObject.FindObjectsOfType<ActorSlot>();

        // Create the player controlled character
        Actor playerActor = InitActor(0, BattleTeam.Player);
        player = GetComponent<Player>();
        player.BattleStart(playerActor);

        InitBattle();
    }

    public Actor GetPlayer()
    {
        return actors[0];
    }


    private Actor InitActor(int id, BattleTeam team)
    {
        ActorSlot start = GetBestStartFor(team);
        if(start == null)
        {
            return null;
        }

        GameObject actor = GameObject.Instantiate(actorPrototype, start.transform.position, Quaternion.identity);
        actor.name = team.ToString();

        Actor a = actor.GetComponent<Actor>();
        a.Init(id, team);
        actors.Add(a);

        start.ActorStarted(a);

        return a;
    }


    private void InitBattle()
    {
        // TODO: initialize the battle from the location node data?
        InitActor(1, BattleTeam.Opponent);
    }


    /// <summary>
    /// Gives the location for the player index to start in
    /// </summary>
    public ActorSlot GetBestStartFor(BattleTeam team)
    {
        ActorSlot bestStart = null;
        foreach(ActorSlot slot in actorSlots)
        {
            if(slot.currentActor == null && slot.team == team)
            {
                bestStart = slot;
                break;
            }
        }        

        return bestStart;
    }


    public IEnumerable<Actor> Opponents()
    {
        foreach(Actor a in actors)
        {
            if(a.team == BattleTeam.Opponent)
            {
                yield return a;
            }
        }
    }

    public IEnumerable<Actor> Friendly()
    {
        foreach(Actor a in actors)
        {
            if(a.team == BattleTeam.Player)
            {
                yield return a;
            }
        }
    }


    public void PlayerEndTurn()
    {
        Debug.Log("Player turn end...");

        foreach(Card card in player.GetDeck().Cards())
        {
            card.ProcessTurn();
        }

        foreach(Actor a in actors)
        {
            a.OnTurnStart();
        }
    }
}
