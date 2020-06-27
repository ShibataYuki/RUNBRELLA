using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INewsUIState
{
    void Entry(int ID);

    void Do(int ID);

    void Exit(int ID);
}
