using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GamepadInput;

public class SkipBehaviour : PlayableBehaviour
{
    public PlayableDirector LineDirector { get; set; } = null;
    public PlayableDirector AudioLineDirector { get; set; } = null;

    /// <summary>
    /// クリップ再生中毎フレーム呼び出される処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        Debug.Log("kiteruyo");
        var no1Player = GameManager.Instance.playerRanks[0];
        var no1Controller = GameManager.Instance.PlayerNoToControllerNo(no1Player);
        // キー入力があればクリップの終わりまで時間を進める
        if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)(no1Controller)))
        {
            GoToClipEnd(playable);
        }
        
    }

    /// <summary>
    /// クリップの再生終了時刻へ時間を飛ばす処理
    /// </summary>
    /// <param name="playable"></param>
    private void GoToClipEnd(Playable playable)
    {
        // クリップ全体の時間
        var clipDuration = playable.GetDuration();
        // 経過時間
        var clipNowTime = playable.GetTime();
        LineDirector.time += clipDuration - clipNowTime;
        AudioLineDirector.time += clipDuration - clipNowTime;

    }
}
