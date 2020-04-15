using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScroll : MonoBehaviour
{
    // スクロールビューのコンポーネント
    private ScrollRect scrollRect = null;

    // スクロールの速度
    private float scrollSpeed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        // 読み込むテキストの名前
        var fileName = string.Format("{0}Data", transform.parent.name);
        // テキストからスクロールの速度を読み込む
        scrollSpeed = TextManager.Instance.GetValue_float(fileName, nameof(scrollSpeed));

        // コンポーネントの取得
        scrollRect = GetComponent<ScrollRect>();
        // コルーチンの開始
        StartCoroutine(RoopScroll());
    }

    /// <summary>
    /// ループするスクロール
    /// </summary>
    /// <returns></returns>
    IEnumerator RoopScroll()
    {
        // スクロールの値
        var value = scrollRect.horizontalNormalizedPosition;

        while(true)
        {
            // 値の変更
            value += Time.deltaTime * scrollSpeed;
            // 0～1の範囲に収める
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            // 値のセット
            scrollRect.horizontalNormalizedPosition = value;
            // スクロールしたら
            if (value >= 1.0f)
            {
                // 値を0に戻す
                value = 0.0f;
            }
            // 次のフレームまで待つ
            yield return null;
        }
    }
}
