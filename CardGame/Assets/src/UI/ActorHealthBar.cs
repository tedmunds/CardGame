using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class ActorHealthBar : MonoBehaviour 
{
    [SerializeField]
    private Text valueField = null;

    [SerializeField]
    private Image fillImage = null;

    private Actor actor;


    public void SetForActor(Actor a)
	{
        this.actor = a;

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if(canvas == null)
        {
            Debug.LogError("Cant add healthbar if there is no canvas!");
            return;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        RectTransform rect = GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(a.transform.position);
        Vector2 canvasPosition = new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));

        rect.anchoredPosition = canvasPosition + new Vector2(0.0f, -25.0f);
    }
	
	protected virtual void Update() 
	{
        if(actor == null)
        {
            return;
        }

        fillImage.fillAmount = (float)actor.health / (float)actor.maxHealth;
        if(valueField != null)
        {
            valueField.text = actor.health + " / " + actor.maxHealth;
        }
    }
}
