using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCoin : MonoBehaviour
{
    // 高さ
    [SerializeField]
    private float height = 0;
    // 補間スピード(フレーム数)
    [SerializeField]
    private float durationSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 曲線移動開始する関数
    /// </summary>
    /// <param name="startPos">開始点</param>
    /// <param name="endPos">終了点</param>
    /// <param name="height">高さ</param>
    /// <param name="durationSpeed">補間スピード</param>
    public void StartCurve(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        // 中点を求める
        Vector3 halfPos = (endPos + startPos) / 2f;
        halfPos.y += height;
        // 曲線移動開始
        StartCoroutine(MoveCurve(startPos, endPos, halfPos, durationSpeed));
    }


    /// <summary>
    /// 補間作業をする関数
    /// </summary>
    /// <param name="startPos">開始点</param>
    /// <param name="endPos">終了点</param>
    /// <param name="halfPos">中点</param>
    /// <param name="t">補間をする割合</param>
    /// <returns></returns>
    Vector3 CalcLerp(Vector3 startPos,Vector3 endPos,Vector3 halfPos,float t)
    {
        // 開始点と中点の補間
        Vector3 aPos = Vector3.Lerp(startPos, halfPos, t);
        // 中点と終了点の補間
        Vector3 bPos = Vector3.Lerp(halfPos, endPos, t);
        // aとbの補間
        return Vector3.Lerp(aPos, bPos, t);
    }

    /// <summary>
    /// 曲線移動をするコルーチン
    /// </summary>
    /// <param name="startPos">開始点  </param>
    /// <param name="endPos">終了点</param>
    /// <param name="halfPos">中点</param>
    /// <param name="durationSpeed">補間スピード</param>
    /// <returns></returns>
    IEnumerator MoveCurve(Vector3 startPos, Vector3 endPos, Vector3 halfPos, float durationSpeed)
    {
        float frameCount = 0;
        float rate = 0;
        while(true)
        {
            // 規定フレームたったら終了
            if(rate>=1.0f)
            {
                yield break;
            }
            rate = frameCount / durationSpeed;
            // 補間した座標をセット
            transform.position = CalcLerp(startPos, endPos, halfPos, rate);
            // フレームカウント
            frameCount++;
            yield return null;
        }
    }
}
