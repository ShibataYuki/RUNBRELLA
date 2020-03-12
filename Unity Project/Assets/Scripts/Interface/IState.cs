using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// 開始処理
    /// </summary>
    /// <param name="ID"></param>
    void Entry(int ID);
       
    /// <summary>
    /// UpDateで呼ばれる処理
    /// </summary>
    /// <param name="ID"></param>
    void Do(int ID);

    /// <summary>
    /// FixUpdateで呼ばれる処理
    /// </summary>
    /// <param name="ID"></param>
    void Do_Fix(int ID);

    /// <summary>
    /// 終了処理
    /// </summary>
    /// <param name="ID"></param>
    void Exit(int ID);

}
