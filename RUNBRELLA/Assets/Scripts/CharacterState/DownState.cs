using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DownState : CharacterState
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
    public override void Entry()
    {
        playerDown.StartDown();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public override void Do()
    {
        // ジャンプボタンを押したらダウン時間を短くする
        //if (InputManager.Instance.JumpKeyIn(ID))
        //{
        //    SceneController.Instance.playerEntityData.playerDowns[ID].nowTime +=
        //        SceneController.Instance.playerEntityData.playerDowns[ID].addTime;
        //}
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
    public override void Do_Fix()
    {
        // SceneController.Instance.playerEntityData.playerRuns[ID].Run();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // 終了処理
        playerDown.EndDown();
    }
}
