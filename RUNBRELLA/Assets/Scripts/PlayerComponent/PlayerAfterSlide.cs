using System.Collections;
using UnityEngine;

public class PlayerAfterSlide : MonoBehaviour
{   
    // ジャンプ受付時間
    [SerializeField]
    float jumpableTime = 0;
    [SerializeField]
    public float catchSliderTime_SlideToSlide = 0f;    
    // タイマーコルーチン
    IEnumerator limitTimer;
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        SheetToDictionary.Instance.TextToDictionary("Chara_Common", out var textDataDic);        
        jumpableTime = textDataDic["手すりを離してからジャンプを受け付ける時間(秒)"];
        catchSliderTime_SlideToSlide = textDataDic["手すりを離してからつかむ判定が出ている継続時間(秒)"];
    }

    /// <summary>
    /// タイマーコルーチン開始処理
    /// </summary>
    /// <param name="controllerNo"></param>
    public void StartTimer()
    {                
        // コルーチンセット
        limitTimer = BreakStateTimer(jumpableTime);
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
    private IEnumerator BreakStateTimer(float limitTime)
    {       
        // 指定時間待機
        yield return new WaitForSeconds(limitTime);
        // 空中状態に移行
        character.AerialStart();
    }
}
