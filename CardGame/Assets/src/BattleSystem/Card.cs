using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour 
{
    // TODO: pull the stats and functionality from somewhere else
    public int energyUse = 5;
    public int cooldown = 1;

    private int slot = -1;
    private Actor owner;

    private SpriteRenderer cardSprite;
    private Vector3 cardBaseScale;
    private Vector3 slotLocation;

    private bool isDragging;
    private Option<int> cooldownRemaining;


    public void Spawn(int slot, Actor owner)
    {
        this.slot = slot;
        this.owner = owner;
    }

	protected virtual void Start() 
	{
        isDragging = false;
        cardBaseScale = transform.localScale;
        slotLocation = transform.position;
        cardSprite = GetComponent<SpriteRenderer>();
    }
	
	protected virtual void Update() 
	{
		if(isDragging)
        {
            if(Input.GetButton("Fire1"))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0.0f;
                transform.position = pos;
            }
            else
            {
                OnCardReleased();

                transform.position = slotLocation;
                isDragging = false;
                OnMouseExit();
            }
        }

	}

    protected virtual void OnMouseEnter()
    {
        if(!CanUse())
        {
            return;
        }

        cardSprite.color = new Color(0.5f, 0.5f, 0.5f);
        StartCoroutine(ScaleTo(cardBaseScale * 1.2f, 0.1f));
    }

    protected virtual void OnMouseExit()
    {
        if(isDragging || !CanUse())
        {
            return;
        }

        cardSprite.color = new Color(1.0f, 1.0f, 1.0f);
        StartCoroutine(ScaleTo(cardBaseScale, 0.1f));
    }

    protected virtual void OnMouseDown()
    {
        if(Input.GetButtonDown("Fire1") && !isDragging && CanUse())
        {
            isDragging = true;
        }
    }
    

    public IEnumerator ScaleTo(Vector3 targetScale, float overTime)
    {
        Vector3 startScale = transform.localScale;
        for(float t = overTime; t >= 0.0f; t -= Time.deltaTime)
        {
            float alpha = 1.0f - (t / overTime);
            Vector3 newScale = Vector3.Lerp(startScale, targetScale, alpha);

            transform.localScale = newScale;
            yield return null;
        }
    }


    public bool CanUse()
    {
        return !cooldownRemaining.IsValid;
    }

    public List<Actor> GetValidTargets()
    {
        var targets = new List<Actor>();
        targets.AddRange(Battle.instance.Opponents());
        return targets;
    }


    private void OnCardReleased()
    {
        Actor bestTarget = null;
        float bestDist = float.MaxValue;
        foreach(Actor target in GetValidTargets())
        {
            float dist = (transform.position - target.transform.position).magnitude;
            if(dist < bestDist)
            {
                bestDist = dist;
                bestTarget = target;
            }
        }

        if(bestTarget == null)
        {
            return;
        }

        // Check useage requirements
        if(owner.energy < energyUse)
        {
            return;
        }

        Activate(bestTarget);
    }
    

    public void Activate(Actor target)
    {
        Debug.Log(slot + " used on " + target.name);

        // consume energy from the user
        owner.ConsumeEnergy(energyUse);

        if(cooldown > 0)
        {
            cooldownRemaining = cooldown;
        }
        
        if(cooldownRemaining.IsValid)
        {
            cardSprite.color = new Color(0.5f, 0.5f, 0.5f);
            StartCoroutine(ScaleTo(cardBaseScale, 0.1f));
        }      
    }


    public void ProcessTurn()
    {
        if(cooldownRemaining.IsValid)
        {
            if(cooldownRemaining == 0)
            {
                cardSprite.color = new Color(1.0f, 1.0f, 1.0f);
                cooldownRemaining.Invalidate();
            }
            else
            {
                cooldownRemaining -= 1;
            }
        }
    }
    

}
