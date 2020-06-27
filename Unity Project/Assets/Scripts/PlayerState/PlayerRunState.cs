using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : RunState
{
    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }
}
