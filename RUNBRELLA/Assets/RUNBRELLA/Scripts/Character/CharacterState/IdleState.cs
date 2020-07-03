using ResultScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour,I_CharacterState
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
    public virtual void Do(){ }

    public virtual void Do_Fix(){ }

    public virtual void Entry(){ }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public virtual void Exit()
    {
        // タイムラインの終了をキャラクターに知らせる
        character.IsTimeLine = false;    
    }
}
