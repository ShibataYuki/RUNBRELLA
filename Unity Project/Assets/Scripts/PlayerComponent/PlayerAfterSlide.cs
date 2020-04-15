using System.Collections;
using System.Collections.Generic;
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
    /// <param name="ID"></param>
    public void StartTimer(int ID)
    {                
        // コルーチンセット
        limitTimer = BreakStateTimer(jumpableTime,ID);
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
    private IEnumerator BreakStateTimer(float limitTime,int ID)
    {       
        // 指定時間待機
        yield return new WaitForSeconds(limitTime);
        // 空中状態に移行
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
    }
}
