using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAnimation : MonoBehaviour
{
    // 拡大アニメーション中のオブジェクト
    private GameObject scalingObject = null;
    // スケーリング中か否のフラグ
    private bool isScaling = false;
    // 1秒間に足される拡大量
    private float scaleUpSpeed = 0.7f;
    public float ScaleUpSpeed { set { scaleUpSpeed = value; } }
    // 拡大前のスケール
    private float minScale = 0.8f;
    public float MinScale { set { minScale = value; } }
    // 拡大後のスケール
    private float maxScale = 1.5f;
    public float MaxScale { set { maxScale = value; } }
    // 拡大アニメーションを行っているIEnumerator
    IEnumerator scaleUpCorutine = null;

    // Update is called once per frame
    void Update()
    {
        // 拡大アニメーションをしていなければ
        if (isScaling == false && scalingObject != null)
        {
            // IEnumeratorをセット
            scaleUpCorutine = ScaleUp();
            // 拡大アニメーションのコルーチンを開始
            StartCoroutine(scaleUpCorutine);
        } // if
    }

    /// <summary>
    /// 拡大アニメーションを行うオブジェクトをセット
    /// </summary>
    /// <param name="setObject">拡大アニメーションを行うオブジェクト</param>
    public void SetScalingObject(GameObject setObject)
    {
        // セットするオブジェクトと別のオブジェクトがセットされていたなら
        if (scalingObject != setObject)
        {
            // 拡大アニメーションを停止
            ScalingStop();
            // 拡大アニメーションを行うオブジェクトをセット
            scalingObject = setObject;
        }
    } // SetScalingObject

    /// <summary>
    /// 拡大アニメーション
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUp()
    {
        // 拡大アニメーションを行うオブジェクトがnullなら
        if(scalingObject == null)
        {
            // コルーチンの終了
            ScalingStop();
            yield break;
        }
        // 拡大中のフラグを立てる
        isScaling = true;
        // 拡大率
        var scale = minScale;
        // スケールの最大と最小の間に収める
        scale = Mathf.Clamp(scale, minScale, maxScale);
        // スケールをセット
        scalingObject.transform.localScale = new Vector3(scale, scale, 1.0f);
        // 次のフレームまで待つ
        yield return null;
        // 毎フレームの拡大処理
        while (true)
        {
            // 拡大縮小するオブジェクトが非表示なら
            if(scalingObject.activeInHierarchy == false)
            {
                // 拡大縮小をストップさせる
                ScalingStop();
                yield break;
            }

            // 時間経過でスケールを大きくする
            scale += scaleUpSpeed * Time.deltaTime;
            // スケールの最大と最小の間に収める
            scale = Mathf.Clamp(scale, minScale, maxScale);
            // スケールをセット
            scalingObject.transform.localScale = new Vector3(scale, scale, 1.0f);
            // 最大まで拡大したなら
            if (scale >= maxScale)
            {
                ScalingStop();
                // コルーチンの終了
                yield break;
            }
            // 次のフレームまで待つ
            yield return null;
        } // while
    } // IEnumerator

    /// <summary>
    /// 拡大アニメーションをストップ
    /// </summary>
    public void ScalingStop()
    {
        //フラグをオフにする
        isScaling = false;
        // コルーチンがnullなら
        if (scaleUpCorutine == null)
        {
            return;
        }
        // コルーチンを停止
        StopCoroutine(scaleUpCorutine);
        // IEnumeratorをnullにする
        scaleUpCorutine = null;
    } // ScalingStop
} // class
