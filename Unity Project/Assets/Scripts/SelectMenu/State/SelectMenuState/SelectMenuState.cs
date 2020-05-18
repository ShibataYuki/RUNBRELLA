using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectMenuState : MonoBehaviour
{
    public abstract void Entry();

    public abstract void Do();

    public abstract void Exit();
}
