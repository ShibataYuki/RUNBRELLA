using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScroll : MonoBehaviour
{
    // スクロールの速度
    private float scrollSpeed = 0f;
    // ワールド座標においてのビューポートのサイズ
    private float viewPortSize;
    
    // Start is called before the first frame update
    void Start()
    {
        // スクロールするスピードをセット
        SetScrollSpeed();
        // ワールド座標においてのビューポートのサイズ
        viewPortSize = (Camera.main.transform.position.x - (Camera.main.ViewportToWorldPoint(Vector3.zero)).x) * 2;
        // コルーチンの開始
        StartCoroutine(RoopScroll());
        // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
        StartCoroutine(RoadSheetCheck());
    }

    /// <summary>
    /// シートの読み込みをチェックして、完了したらパラメータを変更する
    /// </summary>
    /// <returns></returns>
    IEnumerator RoadSheetCheck()
    {
        // シートからの読み込みが完了しているのなら
        if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
        {
            // コルーチンを終了
            yield break;
        }
        while (true)
        {
            // スプレッドシートの読み込みが完了したのなら
            if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
            {
                // パラメータをテキストから読み込んで、speedを変更
                SetScrollSpeed();
                yield break;
            }
            // 1フレーム待機する
            yield return null;
        }
    }

    /// <summary>
    /// スクロールするスピードをセット
    /// </summary>
    private void SetScrollSpeed()
    {
        try
        {
            // ディクショナリーの取得
            SheetToDictionary.Instance.TextToDictionary(SelectMenu.SceneController.Instance.textName, 
                out var scrollTimeDictionary);
            try
            {
                // 背景を1スクロールする時間
                var scrollTime = 1.0f;
                // オブジェクトに応じて、スクロールの時間を変更
                switch (name)
                {
                    case "Town":
                        scrollTime = scrollTimeDictionary["街が1スクロールするのにかかる秒数"];
                        break;
                    case "BackGround_Near":
                        scrollTime = scrollTimeDictionary["街灯とガードレールが1スクロールするのにかかる秒数"];
                        break;
                } // switch
                // スクロールの時間からスピードを算出
                scrollSpeed = 1 / scrollTime;
            } // try
            catch
            {
                Debug.Assert(false, name + "でエラーが発生しました");
            }
        } // try
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から" +
                "Charaselectのディクショナリーを取得できませんでした。");
        }
    } // SetScrollSpeed

    /// <summary>
    /// ループするスクロール
    /// </summary>
    /// <returns></returns>
    IEnumerator RoopScroll()
    {
        // スクロールの値
        var position = transform.position;

        while(true)
        {
            // 値の変更
            position.x -= Time.deltaTime * scrollSpeed * viewPortSize;
            // 画面外に移動したら
            if (position.x < (Camera.main.transform.position.x - viewPortSize))
            {
                // 反対側から出てくる
                position.x += (viewPortSize * 2);
            }
            // ポジションをセット
            transform.position = position;
            // 次のフレームまで待つ
            yield return null;
        }
    }
}
