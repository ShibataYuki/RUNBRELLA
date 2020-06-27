using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSlideState :AfterSlideState
{
    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }
}
