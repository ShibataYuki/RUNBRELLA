using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsUI : MonoBehaviour
{

    // スタート用スプライト
    [SerializeField]
    private Sprite startSprite = null;
    // ゴール用スプライト
    [SerializeField]
    private Sprite goalSprite = null;
    // 全滅勝利用スプライト
    [SerializeField]
    private Sprite winSprite = null;
    // 死亡時用スプライト
    [SerializeField]
    private List<Sprite> deadPlayerSprite = new List<Sprite>();
    // 雨天時用スプライト
    [SerializeField]
    private Sprite rainSprite = null;
    // 待機時間
    [SerializeField]
    private float showTime = 0;
    // NewsUIのImage
    [SerializeField]
    private Image newsImage = null;
    // 表示しているかどうか
    public bool isShowing = false;
    // 移動先の座標
    public Vector3 targetPos;
    // 現在のState
    public INewsUIState nowState = null;
    // このUIのID
    public int ID = 0;

    // Start is called before the first frame update
    void Start()
    {
        newsImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // 今のStateのDoをよぶ
        // nowState.Do(ID);
    }

    /// <summary>
    /// NewsUIを表示する関数
    /// </summary>
    /// <param name="sprite">表示するスプライト</param>
    /// <returns></returns>
    private IEnumerator OnShow(Sprite sprite)
    {
        // 表示中フラグをONにする
        isShowing = true;
        // スプライトを設定
        newsImage.sprite = startSprite;
        // 移動先を計算
        float targetPosX = (-Screen.width / 2f) + (gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f);
        float targetPosY = gameObject.GetComponent<RectTransform>().localPosition.y;
        float targetPosZ = gameObject.GetComponent<RectTransform>().localPosition.z;
        targetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        // 画面内へ移動
        yield return StartCoroutine(OnMove(1f));
        // 規定秒数待機
        yield return new WaitForSeconds(showTime);
        // 移動先を計算
        targetPosX = (-Screen.width / 2f) + (-gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f);
        targetPos = new Vector3(targetPosX, targetPos.y, targetPos.z);
        // 画面外へ移動
        yield return StartCoroutine(OnMove(1f));
        // 移動先を戻す
        targetPos.y = targetPosY;
        var rectPos = gameObject.GetComponent<RectTransform>().localPosition;
        rectPos.y = targetPosY;
        gameObject.GetComponent<RectTransform>().localPosition = rectPos;
        // 表示中フラグをOFFにする
        isShowing = false;
    }


    public IEnumerator OnMove(float targetTime, Vector3 endPos=default)
    {
        var startPos = gameObject.GetComponent<RectTransform>().localPosition;
        float distance = 0;
        Vector3 moveVec;
        if(endPos==default)
        {
            // 移動方向
            moveVec = targetPos - startPos;
            // 目標地点までの距離
            distance = Vector3.Distance(startPos, targetPos);
        }
        else
        {
            // 移動方向
            moveVec = endPos - startPos;
            // 目標地点までの距離
            distance = Vector3.Distance(startPos, endPos);
        }
        // 正規化
        Vector3 normalVec = Vector3.Normalize(moveVec);
        // 現在の移動量
        float nowDistance = 0;
        // 目標の座標に行くまでループ
        while (true)
        {
            // 1フレーム間の移動量を計算する
            float moveValuePer1SecondSpeed = distance / targetTime;
            float moveValuePer1frameSpeed = moveValuePer1SecondSpeed * Time.deltaTime;
            // 移動
            var newsUIPos = gameObject.GetComponent<RectTransform>().position;
            newsUIPos += normalVec * moveValuePer1frameSpeed;
            gameObject.GetComponent<RectTransform>().position = newsUIPos;
            // 移動量を更新
            nowDistance += moveValuePer1frameSpeed;
            // 規定距離以上移動したなら
            if(nowDistance>distance)
            {
                // 位置を修正
                if(endPos==default)
                {
                    gameObject.GetComponent<RectTransform>().localPosition = targetPos;
                }
                else
                {
                    gameObject.GetComponent<RectTransform>().localPosition = endPos;
                }
                // 終了
                yield break;
            }
            yield return null;
        }
    }


    #region ニュース演出の種類の関数


    /// <summary>
    /// スタート時のニュース演出
    /// </summary>
    public void ShowStartNews()
    {
        // 表示
        StartCoroutine(OnShow(startSprite));
    }

    /// <summary>
    /// ゴール時のニュース演出
    /// </summary>
    public void ShowGoalNews()
    {
        // 表示
        StartCoroutine(OnShow(goalSprite));
    }

    /// <summary>
    /// 全滅勝利時のニュース演出
    /// </summary>
    public void ShowWinNews()
    {
        // 表示
        StartCoroutine(OnShow(winSprite));
    }


    /// <summary>
    /// プレイヤー死亡時のニュース演出
    /// </summary>
    /// <param name="gameObject"></param>
    public void ShowDeadPlyaerNews(GameObject gameObject)
    {
        // スプライトを設定
        Sprite sprite = deadPlayerSprite[(int)gameObject.GetComponent<Player>().charType];
        // 表示
        StartCoroutine(OnShow(sprite));
    }

    /// <summary>
    /// 雨天時のニュース演出
    /// </summary>
    public void ShowRainNews()
    {
        // 表示
        StartCoroutine(OnShow(rainSprite));
    }

    #endregion
}
