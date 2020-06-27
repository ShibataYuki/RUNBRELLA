using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuleBook : MonoBehaviour
{
    abstract public void Entry();

    abstract public void Do();

    abstract public void Exit();
}
