using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SelectMenuState
{
    void Entry(SelectMenuState beforeState);

    void Do();

    void Exit(SelectMenuState newState);
}
