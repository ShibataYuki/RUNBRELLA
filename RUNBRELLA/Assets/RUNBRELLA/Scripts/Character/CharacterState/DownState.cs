using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DownState : MonoBehaviour,I_CharacterState
{
    // 必要なコンポーネント
    protected PlayerDown playerDown;
    protected Character character;
    protected RunState runState;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントの取得
        playerDown = GetComponent<PlayerDown>();
        character = GetComponent<Character>();
        runState = GetComponent<RunState>();
    }

    /// <summary>
    /// ステートの開始処理
    /// </summary>
    public virtual void Entry()
    {
        playerDown.StartDown();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public virtual void Do()
    {        
        // 一定時間経過したらダウン状態解除
        if (playerDown.
            TimeCounter(character.downTime))
        {
            // 地面に接地しているなら
            if (character.IsGround == true)
            {
                // 走り状態に移行
                character.RunStart();
            }
            else
            {
                // 空中状態に移行
                character.AerialStart();
            }
        }
    }

    /// <summary>
    /// 物理挙動のフレーム更新処理
    /// </summary>
    public virtual void Do_Fix()
    {        
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public virtual void Exit()
    {
        // 終了処理
        playerDown.EndDown();
    }
}
