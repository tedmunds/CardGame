using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the base data associated with any ability, but little of the actual functionality (just the framework)
/// </summary>
public class Ability 
{
    public delegate Actor[] TargetSlectionDelegate(Vector3 playLocation);
    public delegate void ApplyEffectsDelegate(Actor user, Actor target);

    /// <summary>
    /// Abilities are tagged to let other abilities effect them
    /// </summary>
    public enum Tag
    {
        Attack,
        Skill,
        Spell
    }

    private List<Tag> tags = new List<Tag>();

    public int energyUse { get; private set; }
    public int cooldown { get; private set; }

    private TargetSlectionDelegate targetSelector;
    private ApplyEffectsDelegate applyEffects;

    public Ability()
    {
        // TEMP: 
        tags.Add(Tag.Attack);
        energyUse = 5;
        cooldown = 1;
        targetSelector = OpponentSingle;
        applyEffects = ApplyDamage;
    }

    public IEnumerable<Tag> Tags()
    {
        foreach(Tag t in tags)
        {
            yield return t;
        }
    }

    public bool HasTag(Tag tag)
    {
        return tags.Contains(tag);
    }

    /// <summary>
    /// Called when the ability is played in game, this is after it has been validated
    /// </summary>
    public virtual void Activated(Actor user, Actor[] targets)
    {
        foreach(Actor a in targets)
        {
            applyEffects.Invoke(user, a);
        }
    }

    /// <summary>
    /// Retturns the set of actors that CAN be targetted by this ability based on its target selector function
    /// </summary>
    public virtual Actor[] GetValidTargets(Vector3 position)
    {
        return targetSelector.Invoke(position);       
    }


    private Actor[] OpponentSingle(Vector3 position)
    {
        Actor bestTarget = null;
        float bestDist = float.MaxValue;
        foreach(Actor target in Battle.instance.Opponents())
        {
            float dist = (position - target.transform.position).magnitude;
            if(dist < bestDist)
            {
                bestDist = dist;
                bestTarget = target;
            }
        }

        return new Actor[] { bestTarget };
    }


    private Actor[] PlayerSelf(Vector3 position)
    {
        return new Actor[] { Battle.instance.GetPlayer() };
    }


    private void ApplyDamage(Actor attacker, Actor target)
    {
        target.RecieveDamage(33);
    }


}
