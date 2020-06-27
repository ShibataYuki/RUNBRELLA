using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : AerialState
{
    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }

}
