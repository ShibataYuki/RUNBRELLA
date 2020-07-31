using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSlide : MonoBehaviour
{   
    // ジャンプ受付時間
    [SerializeField]
    float jumpableTime = 0;
    // 手すりから離れてから自動で手すりをつかむ時間
    [SerializeField]
    public float catchSliderTime_SlideToSlide = 0f;    
    // タイマーコルーチン
    IEnumerator exitStateTimer;
    // キャラクター
    Character character;

    private void Start()
    {
        // コンポーネント取得
        character = GetComponent<Character>();
        // テキストからデータ読み込み
        Dictionary<string, float> textDataDic;
        textDataDic = SheetToDictionary.TextNameToData["Chara_Common"];
             
        jumpableTime = textDataDic["手すりを離してからジャンプを受け付ける時間(秒)"];
        catchSliderTime_SlideToSlide = textDataDic["手すりを離してからつかむ判定が出ている継続時間(秒)"];
    }

    /// <summary>
    /// タイマーコルーチン開始処理
    /// </summary>
    /// <param name="controllerNo"></param>
    public void ExitStateTimer_Start()
    {                
        // コルーチンセット
        exitStateTimer = ExitStateTimer(jumpableTime);
        // タイマー開始
        StartCoroutine(exitStateTimer);
    }

    /// <summary>
    /// タイマーコルーチンの終了処理
    /// </summary>    
    public void StopTimer()
    {
        // タイマーをストップ
        StopCoroutine(exitStateTimer);
    }
   
    /// <summary>
    /// 指定時間経過後、空中状態に移行するタイマー
    /// </summary>
    /// <param name="limitTime"></param>
    /// <returns></returns>
    private IEnumerator ExitStateTimer(float limitTime)
    {       
        // 指定時間待機
        yield return new WaitForSeconds(limitTime);
        // 空中状態に移行
        character.AerialStart();
    }
}
