using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn {

    public float dodgeChance;

    public int block;

    public int actorId;

    public Turn(int id)
    {
        actorId = id;
        dodgeChance = 0.0f;
        block = 0;        
    }
}
