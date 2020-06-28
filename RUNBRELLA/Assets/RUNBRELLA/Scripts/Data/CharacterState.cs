using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerとEnemyにアタッチするステートの親クラス(抽象クラス)
/// </summary>
public abstract class CharacterState : MonoBehaviour
{
    // ステートの開始処理
    public abstract void Entry();
    // ステート中のフレーム更新処理
    public abstract void Do();
    // ステート中の物理処理用フレーム更新処理
    public abstract void Do_Fix();
    // ステートの終了時に行う処理
    public abstract void Exit();
}
