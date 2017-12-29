using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EnergyBar : MonoBehaviour 
{
    [SerializeField]
    private Text valueField = null;

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
            fillImage.fillAmount = (float)player.energy / (float)player.maxEnergy;
            valueField.text = player.energy + " / " + player.maxEnergy;
        }
	}
}
