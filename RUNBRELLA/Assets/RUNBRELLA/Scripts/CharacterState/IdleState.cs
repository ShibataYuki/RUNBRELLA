using ResultScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharacterState
{
    // タイムライン中かどうかをチェックするキャラクター
    protected Character character;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントの取得
        character = GetComponent<Character>();
    }
    public override void Do(){ }

    public override void Do_Fix(){ }

    public override void Entry(){ }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // タイムラインの終了をキャラクターに知らせる
        character.IsTimeLine = false;    
    }
}
