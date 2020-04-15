using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
// A behaviour that is attached to a playable
public class LoopBehaviour : PlayableBehaviour
{
    
    // タイムラインコントローラー
    public PlayableDirector director;
    // プレイアブルディレクター
    public ResultScene.TimelineController timelineController;

    /// <summary>
    /// クリップ再生時の処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // タイムラインコントローラーのループ中フラグをON
        timelineController.IsLooping = true;
    }

    /// <summary>
    /// クリップ再生中毎フレーム呼び出される処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        // ループ終了指示が出たらクリップの終わりまで時間を飛ばす
        var stopLoop = timelineController.IsLooping != true;
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
        var looping = timelineController.IsLooping;
        if (looping)
        {            
            director.time -= playable.GetDuration();
            return;
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
        director.time += clipDuration - clipNowTime;
    }
}
