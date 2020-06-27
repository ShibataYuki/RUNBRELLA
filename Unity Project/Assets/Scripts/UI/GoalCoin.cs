using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class GoalCoin : MonoBehaviour
{
    // 高さ
    private float height = 0;
    // 補間スピード(フレーム数)
    private float durationSpeed = 0;
    // コインを表示するポジション
    public Vector3 showPos = new Vector3(0, 0, 0);
    // スタート時の移動スピード(フレーム)
    public float startMoveSpeed = 0;
    // スタート時の拡大率
    public float startCoinSizeMax = 0;
    // 終了時の移動スピード(フレーム)
    public float endMoveSpeed = 0;
    // 終了時の拡大率
    public float endCoinSizeMini = 0;
    // リザルトUI
    [SerializeField]
    private ResultUI resultUI = null;
    // Start is called before the first frame update
    void Start()
    {
        ReadTextParameter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "MiddleResult";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> GoalCoinDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out GoalCoinDictionary);
        try
        {
            // ファイル読み込み
            showPos.x = GoalCoinDictionary["ゴール時に出てくるコインが止まる座標の横(X)方向"];
            showPos.y = GoalCoinDictionary["ゴール時に出てくるコインが止まる座標の縦(Y)方向"];
            startMoveSpeed = GoalCoinDictionary["ゴールコインが旗から出て真ん中に行くまでの時間(フレーム数)"];
            startCoinSizeMax = GoalCoinDictionary["ゴールコインの最大サイズ"];
            endMoveSpeed = GoalCoinDictionary["ゴールコインが真ん中からUIに行くまでの時間(フレーム数)"];
            endCoinSizeMini = GoalCoinDictionary["ゴールコインの最小サイズ"];
        }
        catch
        {
            Debug.Assert(false, nameof(GoalCoin) + "でエラーが発生しました");
        }

    }



    /// <summary>
    /// 移動しながら拡大、縮小をする関数
    /// </summary>
    /// <param name="targetPos">目標地点</param>
    /// <param name="targetFrame">移動にかけるフレーム数</param>
    /// <param name="coinSizeMax">コインが拡大・縮小するサイズ</param>
    /// <returns></returns>
    public IEnumerator OnMove(Vector3 targetPos, float targetFrame, float coinSizeMax)
    {
        // 1フレームでの移動量と拡大量を計算
        Vector3 vec = targetPos - transform.localPosition;
        // 移動方向
        Vector3 normalVec = vec.normalized;
        // 移動距離
        float distance = Vector3.Distance(transform.localPosition, targetPos);
        // 1フレームでの移動量
        float speed = distance / targetFrame;
        // 1フレームでの拡大量
        float expansion = 
            (coinSizeMax - gameObject.GetComponent<RectTransform>().sizeDelta.x) / targetFrame;
        float nowDistance = 0;
        // 目標地点まで繰り返す
        while (true)
        {
            // 拡大処理
            gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(1, 1) * expansion;
            // 移動処理
            gameObject.GetComponent<RectTransform>().localPosition += normalVec * speed;
            nowDistance += speed;
            if(nowDistance>=distance)
            {
                // 本来のサイズにする
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1) * coinSizeMax;
                // 本来の位置にする
                gameObject.GetComponent<RectTransform>().localPosition = targetPos;
                yield break;
            }
            if(GamePad.GetButtonDown(GamePad.Button.A,GamePad.Index.Any)||Input.GetKeyDown(KeyCode.Return))
            {
                // 終了処理
                End();
                yield break;
            }
            yield return null;
        }
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public void End()
    {
        // 本来のサイズにする
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1) * endCoinSizeMini;
        // 座標を勝ったプレイヤーのコイン用UIと同じ座標にする
        gameObject.GetComponent<RectTransform>().localPosition = resultUI.targetPos;
        // 回転アニメーション停止
        resultUI.goalCoinAnimator.SetBool("isStart", false);
        // スタンプのSE再生
        AudioManager.Instance.PlaySE(resultUI.StampSE, 0.5f);
        // リザルトコルーチンを終了する
        StopCoroutine(resultUI.resultCoroutine);
        // 終了フラグをONにする
        resultUI.isEnd = true;
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
        Vector3 startPos = GetComponent<Camera>().ScreenToWorldPoint(transform.position);
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
            transform.position = GetComponent<Camera>().WorldToScreenPoint(CalcLerp(startPos, endPos, halfPos, rate));
            // フレームカウント
            frameCount++;
            yield return null;
        }
    }
}
