using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_StateManager
{
    /// <summary>
    /// 状態を変更する処理
    /// </summary>
    /// <param name="a"></param>
    /// <param name="controllerNo"></param>
    void ChangeState(I_State a, CONTROLLER_NO controllerNo);

}
