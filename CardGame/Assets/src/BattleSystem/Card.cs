using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour 
{
    private int slot = -1;
    private Actor owner;

    private Ability cardAbility;

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

        // TEMP: 
        cardAbility = new Ability();
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
        // Check useage requirements
        if(owner.energy < cardAbility.energyUse)
        {
            return;
        }

        Activate(cardAbility.GetValidTargets(transform.position));
    }
    

    public void Activate(Actor[] targets)
    {
        // consume energy from the user
        owner.ConsumeEnergy(cardAbility.energyUse);
        
        cardAbility.Activated(owner, targets);

        if(cardAbility.cooldown > 0)
        {
            cooldownRemaining = cardAbility.cooldown;
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
