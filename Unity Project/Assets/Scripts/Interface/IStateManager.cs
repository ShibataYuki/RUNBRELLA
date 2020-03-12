using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateManager
{
    /// <summary>
    /// 状態を変更する処理
    /// </summary>
    /// <param name="a"></param>
    /// <param name="ID"></param>
    void ChangeState(IState a, int ID);


}
