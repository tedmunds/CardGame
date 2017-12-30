using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void CardEffect(Actor activator, Actor target, Card source);

public enum BattleTeam
{
    Player,
    Opponent
}

public enum BattleEndState
{
    Victory,
    Death,
    Failure // Maybe?
}



[RequireComponent(typeof(Player))]
public class Battle : MonoBehaviour
{
    public static Battle instance { get; private set; }

    [SerializeField]
    private GameObject actorPrototype = null;

    [SerializeField]
    private GameObject healthBarPrototype = null;
    
    private ActorSlot[] actorSlots;

    // All players in the battle, can be AI or PC
    private List<Actor> actors = new List<Actor>();

    private Player player;
    private List<Enemy> enemies = new List<Enemy>();
    
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
        for(int i = 0; i < 2; i++)
        {
            Actor opponent = InitActor(i + 1, BattleTeam.Opponent);
            AddHealthBarForActor(opponent, healthBarPrototype);

            opponent.onDeath += OnOpponentKilled;

            Enemy e = opponent.gameObject.AddComponent<Enemy>();
            enemies.Add(e);

            // the enemy controlls this actor
            e.OnSpawn(opponent);
        }
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


    public void OnOpponentKilled(Actor killed)
    {
        bool opponentsAreAlive = false;
        foreach(Actor opponent in Opponents())
        {
            if(!opponent.isDead)
            {
                opponentsAreAlive = true;
            }
        }

        if(!opponentsAreAlive)
        {
            EndBattle(BattleEndState.Victory);
        }
    }


    public void PlayerEndTurn()
    {
        Debug.Log("Player turn end...");

        // Firs process card effects before the enemies turn actually starts
        foreach(Card card in player.GetDeck().Cards())
        {
            card.ProcessTurn();
        }

        // First the opponents start their turn
        foreach(Actor a in Opponents())
        {
            a.OnTurnStart();
        }

        // TODO: a time delay for animations and stuff to callback when the enemy turn is actually over

        // Then the players actors start their turn
        foreach(Actor a in Friendly())
        {
            a.OnTurnStart();
        }
    }


    public void EndBattle(BattleEndState endState)
    {
        switch(endState)
        {
            case BattleEndState.Victory:
                Debug.Log("You've won the battle!");
                break;
            case BattleEndState.Death:
                Debug.Log("You've been killed in brutal combat!");
                break;
        }
        
        // TODO: clean up and go to overworld, or not I guess if you died? go back to "bonfire" in that case I guess
        // Give awards and stuff
    }


    public void AddHealthBarForActor(Actor a, GameObject healthBarProto)
    {
        if(a == null || healthBarProto == null)
        {
            return;
        }

        GameObject hpBar = Instantiate(healthBarProto, FindObjectOfType<Canvas>().transform);
        var healthBarInst = hpBar.GetComponent<ActorHealthBar>();
        if(healthBarInst != null)
        {
            healthBarInst.SetForActor(a);
        }
    }
}
