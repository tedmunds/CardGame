﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HiddenInGame : MonoBehaviour 
{
	
    protected void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
