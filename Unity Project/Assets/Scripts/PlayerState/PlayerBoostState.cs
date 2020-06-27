using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostState : BoostState
{

    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }
}
