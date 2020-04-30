using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charaselect : MonoBehaviour
{
    // 画像のコンポーネント
    Image image;
    // 消えてから表示されるまでの時間
    float showTime = 3.0f;
    // 現れてから消えるまでの時間
    float hideTime = 3.0f;

    // ステート
    enum State
    {
        Show,
        Hide,
    }
    // 現在のステート
    State state = State.Show;

    // コルーチン
    IEnumerator corutine;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // コルーチンがNUllなら
        if(corutine == null)
        {
            // ステートに応じたコルーチンをセット
            switch(state)
            {
                case State.Show:
                    corutine = ShowImage();
                    break;
                case State.Hide:
                    corutine = HideImage();
                    break;
            }
            // コルーチンの開始
            StartCoroutine(corutine);
        }
    }

    /// <summary>
    /// 一定時間後に画像を不透明にする
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowImage()
    {
        // 透明にする
        var color = image.color;
        color.a = 0.0f;
        image.color = color;
        // 一定時間待機する
        yield return new WaitForSeconds(showTime);
        // 不透明にする
        color.a = 1.0f;
        image.color = color;
        // ステートの変更
        state = State.Hide;
        corutine = null;
    } // ShowImage

    /// <summary>
    /// 一定時間後に画像を透明にする
    /// </summary>
    /// <returns></returns>
    IEnumerator HideImage()
    {
        // 不透明にする
        var color = image.color;
        color.a = 1.0f;
        image.color = color;
        // 一定時間待機する
        yield return new WaitForSeconds(hideTime);
        // 透明にする
        color.a = 0.0f;
        image.color = color;
        // ステートの変更
        state = State.Show;
        corutine = null;
    } // HideImage

    /// <summary>
    /// 画像を非表示にする
    /// </summary>
    public void Close()
    {
        // ステートを変更
        state = State.Show;
        // 透明にする
        var color = image.color;
        color.a = 0.0f;
        image.color = color;
        // コルーチンが入っているなら
        if(corutine != null)
        {
            // コルーチンの終了
            StopCoroutine(corutine);
            // コルーチンを削除
            corutine = null;
        }
        // 非表示にする
        gameObject.SetActive(false);
    } // Close
} // class