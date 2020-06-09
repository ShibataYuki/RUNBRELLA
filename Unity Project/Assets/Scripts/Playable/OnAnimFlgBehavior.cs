using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GamepadInput;

public class OnAnimFlgBehavior : PlayableBehaviour
{
    /// <summary>
    /// クリップ再生時の処理
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        var topPlayerNo = GameManager.Instance.playerResultInfos[0].playerNo;
        // 一位のplayerのオブジェクトを取得する
        var topPlayerObj = SceneController.Instance.playerObjects[topPlayerNo];
        // アニメーターのフラグを切り替える
        var animator = topPlayerObj.GetComponent<Animator>();
        animator.SetBool("isStaging", true);                
    }
        
}
