using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour 
{
    [SerializeField]
    private Text textField = null;

    private Image fillImage;
    private Actor player;

    protected virtual void Start()
    {
        fillImage = GetComponent<Image>();
    }

    protected virtual void Update()
    {
        if(player == null)
        {
            player = Battle.instance.GetPlayer();
        }
        else
        {
            fillImage.fillAmount = (float)player.health / (float)player.maxHealth;
            textField.text = player.health + " / " + player.maxHealth;
        }
    }
}
