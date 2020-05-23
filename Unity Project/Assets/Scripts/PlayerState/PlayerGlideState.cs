using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : GlideState
{
    protected override void Start()
    {
        base.Start();
        character = GetComponent<Player>();
    }
}
