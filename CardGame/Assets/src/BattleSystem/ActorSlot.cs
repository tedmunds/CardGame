using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSlot : MonoBehaviour 
{
    [SerializeField]
    public BattleTeam team;
    
    public Actor currentActor { get; private set; }

    public void ActorStarted(Actor newActor)
    {
        currentActor = newActor;
    }
}
