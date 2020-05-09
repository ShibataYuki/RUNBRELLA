using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NEWSMODE
{
    START,
    GOAL,
    WIN,
    DEAD,
    RAIN,
}


public class NewsUIEntry : MonoBehaviour
{
    // newsUIに使用するSpriteの構造体
    [System.Serializable]
    public struct NewsUISprite
    {
        public Sprite startSprite;
        public List<Sprite> goalSprite;
        public List<Sprite> winSprite;
        public List<Sprite> deadSprite;
        public Sprite rainSprite;
    }

    public NewsUISprite newsUISprite;

    // Y座標のオフセット
    [SerializeField]
    float offsetY = 0;
    // 表示するNEWSMODE
    public NEWSMODE newsMode;
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
    // 拡大時の大きさ
    [SerializeField]
    Vector2 sizeMax;
    // 次のState
    NewsUIShowState showState;
    // 死んだプレイヤーのプレイヤーナンバー
    public PLAYER_NO playerNo;
    // サイズの差分
    Vector2 sizeDiference;


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
        Dictionary<string, float> NewsUIEntryDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out NewsUIEntryDictionary);
        try
        {
            // ファイル読み込み
            offsetY = NewsUIEntryDictionary["画面の一番上とニュース演出の間の距離"];
            entryTime = NewsUIEntryDictionary["ニュース演出が出てき終わるまでの時間"];
            sizeMax.x = NewsUIEntryDictionary["ニュース演出が出てくるときに大きくなる大きさの横(X)"];
            sizeMax.y = NewsUIEntryDictionary["ニュース演出が出てくるときに大きくなる大きさの縦(Y)"];
        }
        catch
        {
            Debug.Assert(false, nameof(NewsUIEntry) + "でエラーが発生しました");
        }

    }



    /// <summary>
    /// EntryStateのEntry処理をする関数
    /// </summary>
    public void StartEntry()
    {
        // 初期化処理
        showState = NewsUIManager.Instance.gameObject.GetComponent<NewsUIShowState>();
        // スプライトの設定
        var newsUIImage = gameObject.GetComponent<Image>();
        switch (newsMode)
        {
            case NEWSMODE.DEAD:
                newsUIImage.sprite = newsUISprite.deadSprite[(int)playerNo];
                break;
            case NEWSMODE.GOAL:
                newsUIImage.sprite = newsUISprite.goalSprite[(int)playerNo];
                break;
            case NEWSMODE.RAIN:
                newsUIImage.sprite = newsUISprite.rainSprite;
                break;
            case NEWSMODE.START:
                newsUIImage.sprite = newsUISprite.startSprite;
                break;
            case NEWSMODE.WIN:
                newsUIImage.sprite = newsUISprite.winSprite[(int)playerNo];
                break;
        }
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        sizeDiference.x = sizeMax.x - gameObject.GetComponent<RectTransform>().sizeDelta.x;
        sizeDiference.y = sizeMax.y - gameObject.GetComponent<RectTransform>().sizeDelta.y;
        // スタート地点を設定
        startPos = newsUIRect.localPosition;
        startPos.y = (Screen.height / 2f) - (newsUIRect.sizeDelta.y / 2f) - offsetY;
        newsUIRect.localPosition = startPos;
        // 目標地点を設定
        float targetPosX = (-Screen.width / 2f) + (newsUIRect.sizeDelta.x / 2f) + (sizeDiference.x / 2f);
        float targetPosY = (Screen.height / 2f) - (newsUIRect.sizeDelta.y / 2f) - offsetY;
        float targetPosZ = newsUIRect.localPosition.z;
        targetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        // スタート地点から目標地点までの方向を計算
        var moveVec = targetPos - startPos;
        // 正規化
        normalMoveVec = moveVec.normalized;
        // スタート地点から目標地点までの距離を計算
        distance = Vector3.Distance(startPos, targetPos);
        nowDistance = 0f;

    }

    /// <summary>
    /// EntryStateのDo処理をする関数
    /// </summary>
    public void OnEntry()
    {
        var newsUIRect = gameObject.GetComponent<RectTransform>();
        // 1フレーム間の移動量を計算する
        float moveValuePer1SecondSpeed = distance / entryTime;
        float moveValuePer1FrameSpeed = moveValuePer1SecondSpeed * Time.deltaTime;
        // 1フレーム間の拡大量を計算する
        float growValueXPer1SecondSpeed = sizeDiference.x / entryTime;
        float growValueYPer1SecondSpeed = sizeDiference.y / entryTime;
        float growValueXPer1FrameSpeed = growValueXPer1SecondSpeed * Time.deltaTime;
        float growValueYPer1FrameSpeed = growValueYPer1SecondSpeed * Time.deltaTime;
        // 移動
        var newsUIPos = newsUIRect.localPosition;
        newsUIPos += normalMoveVec * moveValuePer1FrameSpeed;
        newsUIRect.localPosition = newsUIPos;
        // 拡大
        var newsUISize = newsUIRect.sizeDelta;
        newsUISize.x += growValueXPer1FrameSpeed;
        newsUISize.y += growValueYPer1FrameSpeed;
        newsUIRect.sizeDelta = newsUISize;
        // 移動量を更新
        nowDistance += moveValuePer1FrameSpeed;
        // 規定距離以上移動したなら
        if (nowDistance > distance)
        {
            // X方向のみ位置を修正
            var rectPos = newsUIRect.localPosition;
            rectPos.x = targetPos.x;
            newsUIRect.localPosition = rectPos;
            // サイズを修正
            newsUIRect.sizeDelta = sizeMax;
            var ID = gameObject.GetComponent<NewsUI>().ID;
            // EntryStateを終了
            NewsUIManager.Instance.ChangeNewsUIState(showState, ID);
        }

    }


    public void EndEntry()
    {
        // 初期化処理

    }

}
