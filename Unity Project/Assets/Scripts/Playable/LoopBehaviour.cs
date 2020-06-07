using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GamepadInput;

// A behaviour that is attached to a playable
public class LoopBehaviour : PlayableBehaviour
{

    // プレイアブルディレクター
    public PlayableDirector Director { get; set; }    
    // ループ中フラグ
    public bool IsLooping { get; private set; } = false;

    /// <summary>
    /// クリップ再生時の処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // ループ中フラグをON
        IsLooping = true;
    }

    /// <summary>
    /// クリップ再生中毎フレーム呼び出される処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        var no1Player = GameManager.Instance.playerResultInfos[0].playerNo;
        var no1Controller = GameManager.Instance.PlayerNoToControllerNo(no1Player);
        // キー入力があればクリップの終わりまで時間を進める
        if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)(no1Controller)))
        {
            BreakLoopClip();
        }
        // ループ終了指示が出たらクリップの終わりまで時間を飛ばす
        var stopLoop = IsLooping == false;
        if (stopLoop)
        {
            GoToClipEnd(playable);
        }
    }

    /// <summary>
    /// クリップ終了時に呼び出される処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // まだループするべきなら時間をクリップの開始時刻に戻す        
        if (IsLooping)
        {            
            Director.time -= playable.GetDuration();
            return;
        }        
    }

    /// <summary>
    /// ループクリップを抜ける処理
    /// (実際にはフラグを参照して「LoopBehaviour」が処理を変更する）
    /// </summary>
    public void BreakLoopClip()
    {
        // 現在ループ中でなければリターン
        var notLooping = !IsLooping;
        if (notLooping) { return; }
        IsLooping = false;
    }

    /// <summary>
    /// クリップの再生終了時刻へ時間を飛ばす処理
    /// </summary>
    /// <param name="playable"></param>
    public void GoToClipEnd(Playable playable)
    {
        // クリップ全体の時間
        var clipDuration = playable.GetDuration();
        // 経過時間
        var clipNowTime = playable.GetTime();
        Director.time += clipDuration - clipNowTime;
    }
}
