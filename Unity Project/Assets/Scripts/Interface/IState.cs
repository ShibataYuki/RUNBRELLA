using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// 開始処理
    /// </summary>
    /// <param name="controllerNo"></param>
    void Entry(CONTROLLER_NO controllerNo);
       
    /// <summary>
    /// UpDateで呼ばれる処理
    /// </summary>
    /// <param name="playerNO"></param>
    void Do(CONTROLLER_NO controllerNo);

    /// <summary>
    /// FixUpdateで呼ばれる処理
    /// </summary>
    /// <param name="playerNo"></param>
    void Do_Fix(CONTROLLER_NO controllerNo);

    /// <summary>
    /// 終了処理
    /// </summary>
    /// <param name="playerNo"></param>
    void Exit(CONTROLLER_NO controllerNo);

}
