using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : SlideState
{
    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }
}
