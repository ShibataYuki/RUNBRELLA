using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerとEnemyにアタッチするステートの親クラス(抽象クラス)
/// </summary>
public interface I_CharacterState  
{
    // ステートの開始処理
    void Entry();
    // ステート中のフレーム更新処理
    void Do();
    // ステート中の物理処理用フレーム更新処理
    void Do_Fix();
    // ステートの終了時に行う処理
    void Exit();
}
