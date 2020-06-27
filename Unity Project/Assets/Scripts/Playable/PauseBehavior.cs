using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using GamepadInput;


public class PauseBehavior : PlayableBehaviour
{
    public PlayableDirector Director { get; set; } = null;
    // クリップの開始時間
    double clipEndTime = 0;
    /// <summary>
    /// クリップ再生時の処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {        
        // クリップの開始時間を保持
        var clipStartTime = Director.time;
        clipEndTime = clipStartTime + playable.GetDuration();
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
            GoToClipEnd(playable);           
        }
        else
        {
            // そうでなければクリップの終了時間で止める
            StopBeforeClipEnd();
        }
        
    }
           

    public void StopBeforeClipEnd()
    {
        if(Director.time >=  clipEndTime - 0.1)
        {
            Director.time = clipEndTime - 0.1;
        }
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
