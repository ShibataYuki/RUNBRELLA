using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIExit : MonoBehaviour
{
    // 目標地点
    Vector3 targetPos;
    // スタート地点
    Vector3 startPos;
    // 正規化した移動方向
    Vector3 normalMoveVec;
    // スタート地点から目標地点までの距離
    float distance;
    // 現在の目標地点までの距離
    float nowDistance;
    // Entryの時間
    [SerializeField]
    float entryTime = 0;
    // 次のState
    NewsUIIdleState idleState;
    // 元のサイズ
    [SerializeField]
    Vector2 defaultSize = default;


    private void Start()
    {
        ReadTextParameter();
    }


    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "News";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> NewsUIExitDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out NewsUIExitDictionary);
        try
        {
            // ファイル読み込み
            entryTime = NewsUIExitDictionary["ニュース演出が引っ込むときの引っ込むまでの時間"];
        }
        catch
        {
            Debug.Assert(false, nameof(NewsUIExit) + "でエラーが発生しました");
        }

    }



    // ExitStateのEntry処理をする関数
    public void StartExit()
    {
        // 初期化処理
        idleState = NewsUIManager.Instance.GetComponent<NewsUIIdleState>();
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        // スタート地点を設定
        startPos = newsUIRect.localPosition;
        // 移動先を計算
        float targetPosX = (-Screen.width / 2f) + (-newsUIRect.sizeDelta.x / 2f);
        float targetPosY = newsUIRect.localPosition.y;
        float targetPosZ = newsUIRect.localPosition.z;
        targetPos = new Vector3(targetPosX, targetPosY, targetPosZ); ;
        // スタート地点から目標地点までの方向を計算
        var moveVec = targetPos - startPos;
        // 正規化
        normalMoveVec = moveVec.normalized;
        // スタート地点から目標地点までの距離を計算
        distance = Vector3.Distance(startPos, targetPos);
        nowDistance = 0f;
    }

    /// <summary>
    /// ExitStateのDo処理をする関数
    /// </summary>
    public void OnExit()
    {
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        // 1フレーム間の移動量を計算する
        float moveValuePer1SecondSpeed = distance / entryTime;
        float moveValuePer1frameSpeed = moveValuePer1SecondSpeed * Time.deltaTime;
        // 移動
        var newsUIPos = newsUIRect.localPosition;
        newsUIPos += normalMoveVec * moveValuePer1frameSpeed;
        newsUIRect.localPosition = newsUIPos;
        // 移動量を更新
        nowDistance += moveValuePer1frameSpeed;
        // 規定距離以上移動したなら
        if (nowDistance > distance)
        {
            // X方向のみ位置を修正
            var rectPos = newsUIRect.localPosition;
            rectPos.x = targetPos.x;
            newsUIRect.localPosition = rectPos;
            var ID = gameObject.GetComponent<NewsUI>().ID;
            // EntryStateを終了
            NewsUIManager.Instance.ChangeNewsUIState(idleState, ID);
        }
    }

    /// <summary>
    /// ExitStateのExit処理をする関数
    /// </summary>
    public void EndExit()
    {
        // 終了処理
        // 座標を初期化
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        float targetPosX = (-Screen.width / 2f) + (-newsUIRect.sizeDelta.x / 2f);
        float targetPosY = (Screen.height / 2f) - (newsUIRect.sizeDelta.y / 2f);
        float targetPosZ = newsUIRect.localPosition.z;
        targetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        newsUIRect.localPosition = targetPos;
        // サイズを初期化
        newsUIRect.sizeDelta = defaultSize;

    }
}
