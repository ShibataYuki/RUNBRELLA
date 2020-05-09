using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsUI : MonoBehaviour
{

    // 現在のState
    public INewsUIState nowState = null;
    // このUIのID
    public int ID = 0;
    // NewsUIの何段目か
    public int index = 0;
    // 下に移動しているか
    public bool isMoveingUnder = false;
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
    // 一段ずれるまでの時間
    [SerializeField]
    float moveUnderTime = 0;
    // Y座標のオフセット
    [SerializeField]
    float offsetY = 0;


    // Start is called before the first frame update
    void Start()
    {
        ReadTextParameter();
    }

    // Update is called once per frame
    void Update()
    {
        // 今のStateのDoをよぶ
        nowState.Do(ID);
        // １段下がる
        if(isMoveingUnder)
        {
            MoveUnder();
        }
    }

    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "News";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> NewsUIDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out NewsUIDictionary);
        try
        {
            // ファイル読み込み
            moveUnderTime = NewsUIDictionary["ニュース演出が下に下がる時間"];
            offsetY = NewsUIDictionary["ニュース演出が下に下がったときの上との差分"];
        }
        catch
        {
            Debug.Assert(false, nameof(NewsUI) + "でエラーが発生しました");
        }

    }


    /// <summary>
    /// １段ずれる移動の初期化処理をする関数
    /// </summary>
    public void MoveUnderInit()
    {
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        // スタート地点を設定
        startPos = newsUIRect.localPosition;
        // 目標地点を設定
        float targetPosX = newsUIRect.localPosition.x;
        float targetPosY = newsUIRect.localPosition.y - newsUIRect.sizeDelta.y - offsetY;
        float targetPosZ = newsUIRect.localPosition.z;
        targetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        // スタート地点から目標地点までの方向を計算
        var moveVec = targetPos - startPos;
        // 正規化
        normalMoveVec = moveVec.normalized;
        // スタート地点から目標地点までの距離を計算
        distance = Vector3.Distance(startPos, targetPos);
        nowDistance = 0f;
        // 移動フラグをONにする
        isMoveingUnder = true;
    }


    /// <summary>
    /// １段下にずれる移動処理をする関数
    /// </summary>
    private void MoveUnder()
    {
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        // 1フレーム間の移動量を計算する
        float moveValuePer1SecondSpeed = distance / moveUnderTime;
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
            // Y方向のみ位置を修正
            var rectPos = newsUIRect.localPosition;
            rectPos.y = targetPos.y;
            newsUIRect.localPosition = rectPos;
            // 移動フラグをOFFにする
            isMoveingUnder = false;
        }
    }
}
