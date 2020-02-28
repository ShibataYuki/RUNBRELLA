using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Entry(int ID);

    void Do_Fix(int ID);

    void Do(int ID);

    void Exit(int ID);

}
