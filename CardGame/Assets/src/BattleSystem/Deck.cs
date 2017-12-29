using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour 
{
    [SerializeField]
    private Card cardProxy = null;
    
    private List<Card> cards = new List<Card>();
    private Actor owner;

	public void Init(Actor owner) 
	{
        this.owner = owner;

        // For now just do some auto placement of the cards
        float cardSpacing = 1.7f;
        float leftStart = (cardSpacing * (GameConstants.HandSize - 1)) / 2;
        
        for(int i = 0; i < GameConstants.HandSize; i++)
        {
            var card = Instantiate<Card>(cardProxy, new Vector3(-leftStart + (cardSpacing * i), -3.2f, 0.0f), Quaternion.identity);
            cards.Add(card);
            card.Spawn(i, owner);
        }
	}

    public IEnumerable<Card> Cards()
    {
        foreach(Card c in cards)
        {
            yield return c;
        }
    }
}
