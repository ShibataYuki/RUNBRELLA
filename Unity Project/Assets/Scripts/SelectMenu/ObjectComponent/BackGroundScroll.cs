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
        // 読み込むテキストの名前
        var fileName = string.Format("{0}Data", name);
        // テキストからスクロールの速度を読み込む
        scrollSpeed = TextManager.Instance.GetValue_float(fileName, nameof(scrollSpeed));
        // ワールド座標においてのビューポートのサイズ
        viewPortSize = (Camera.main.transform.position.x - (Camera.main.ViewportToWorldPoint(Vector3.zero)).x) * 2;
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
