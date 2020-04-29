using System.Collections;
using UnityEngine;

public class PlayerAfterSlide : MonoBehaviour
{   
    // ジャンプ受付時間
    [SerializeField]
    float jumpableTime = 0.1f;    
    // タイマーコルーチン
    IEnumerator limitTimer;
   
    /// <summary>
    /// タイマーコルーチン開始処理
    /// </summary>
    /// <param name="controllerNo"></param>
    public void StartTimer(CONTROLLER_NO controllerNo)
    {                
        // コルーチンセット
        limitTimer = BreakStateTimer(jumpableTime,controllerNo);
        // タイマー開始
        StartCoroutine(limitTimer);
    }

    /// <summary>
    /// タイマーコルーチンの終了処理
    /// </summary>    
    public void StopTimer()
    {
        // タイマーをストップ
        StopCoroutine(limitTimer);
    }
   

    /// <summary>
    /// 指定時間経過後、ジャンプ状態に移行するタイマー
    /// </summary>
    /// <param name="limitTime"></param>
    /// <returns></returns>
    private IEnumerator BreakStateTimer(float limitTime,CONTROLLER_NO controllerNo)
    {       
        // 指定時間待機
        yield return new WaitForSeconds(limitTime);
        // 空中状態に移行
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
    }
}
