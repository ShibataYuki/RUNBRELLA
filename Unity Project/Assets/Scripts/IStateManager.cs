using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateManager
{
    

     void ChangeState(IState a, int ID);
}
