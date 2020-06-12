using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class CameraManager : MonoBehaviour
{

    enum PlayerMoveDirection
    {
        RIGHT,
        LEFT,
    }
    public enum CAMERA_MOVEMODE
    {
        TO_GOAL,
        TO_START,
    }

    PlayerMoveDirection playerMoveDirection;

    // 動かすカメラ
    Camera mainCamera;
    // 一番右にいるプレイヤーのx座標
    float firstRightXPos;
    // 一番左にいるプレイヤーのx座標
    float firstLeftXPos = 0;
    // プレイヤーのX座標順のリスト
    List<GameObject> playerXPosOrder = new List<GameObject>();
    // カメラが動く時のカメラと一位のプレイヤーのx座標の距離
    [SerializeField]
    float moveOffset = 10;
    // カメラ演出でゴールフラッグからスタート地点に行くまでの時間
    [SerializeField]
    float moveTime = 0;
    // カメラ演出の移動モード
    [SerializeField]
    CAMERA_MOVEMODE cameraMoveMode = default;
    // カメラ演出の待機時間
    [SerializeField]
    float waitTime = 0;
    // スタート時のスタート地点からのカメラのオフセット
    [SerializeField]
    private const int startCameraPosOffset= 10;
    // 信号機のx座標
    private float trafficLightPosX;

    #region シングルトン
    // シングルトン
    private static CameraManager instance;
    public static CameraManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // textからパラメータを読み込む
        ReadTextParameter();
        // 初期化
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        trafficLightPosX = GameObject.Find("TrafficLight").transform.position.x;
        playerMoveDirection = PlayerMoveDirection.RIGHT;
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            playerXPosOrder.Add(SceneController.Instance.playerObjects[(PLAYER_NO)i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneController.Instance.isStart)
        {
            // 毎フレーム順位をチェックし一位の座標を返す
            CheckRanking(playerMoveDirection);
            if(SceneController.Instance.isStart)
            {
                // カメラを動かす処理
                MoveCamera(firstRightXPos);
            }
        }
    }


    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "Camera";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> cameraManagerDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out cameraManagerDictionary);
        try
        {
            // ファイル読み込み
            moveOffset = cameraManagerDictionary["1位のプレイヤーがカメラを引っ張るまでの距離"];
            moveTime = cameraManagerDictionary["カメラ演出でスタートからゴールに行くまでの時間"];
            waitTime = cameraManagerDictionary["カメラ演出の前と終わりで待機する時間"];
        }
        catch
        {
            Debug.Assert(false, nameof(CameraManager) + "でエラーが発生しました");
        }

    }


    /// <summary>
    /// 一位のx座標を取得する関数
    /// </summary>
    /// <returns></returns>
    void CheckRanking(PlayerMoveDirection moveDirection)
    {

        // プレイヤーのX,Y座標順にソート
        playerXPosOrder.Sort(CompareByPositionX);

        // x,y座標ごとに代入
        // 一位の位置を代入
        firstRightXPos = playerXPosOrder[0].transform.position.x;
        // 最下位の位置を代入
        // playerRankOrderの要素の最後のactiveInHierarchyがtrueとは限らないのでfor文で回す
        for (int i = GameManager.Instance.playerNumber-1; i >= 0; i--)
        {
            if (playerXPosOrder[i].activeInHierarchy)
            {
                firstLeftXPos = playerXPosOrder[i].transform.position.x;
                return;
            }
        }

    }


    /// <summary>
    /// ソートの戻り値がint型だけのためGameObjectのx座標でソートする関数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static int CompareByPositionX(GameObject a, GameObject b)
    {
        // 描画されてないなら一番後ろにする
        //if (!a.activeInHierarchy)
        //{
        //    return -1;
        //}
        if (a.transform.position.x < b.transform.position.x)
        {
            return 1;
        }

        if (a.transform.position.x > b.transform.position.x)
        {
            return -1;
        }

        return 0;
    }


    /// <summary>
    /// カメラの位置を修正する関数
    /// </summary>
    /// <param name="pos"></param>
    void MoveCamera(float pos)
    {
        // カメラとプレイヤーが一定以上離れたらカメラの位置を修正する
        if(pos-mainCamera.transform.position.x>moveOffset)
        {
            // カメラの位置を取得
            Vector3 cameraPos = mainCamera.transform.position;
            cameraPos.x = pos - moveOffset;
            // cameraPos.y = pos.y;
            // カメラの位置を修正
            mainCamera.transform.position = cameraPos;
        }
    }


    public IEnumerator MoveCameraProduction()
    {
        // 開始点
        Vector3 startPos = new Vector3(0, 0, 0);
        // 終了点
        Vector3 endPos = new Vector3(0, 0, 0);
        // 移動方向
        Vector3 moveVec = new Vector3(0, 0, 0);
        // ゴールフラッグを取得
        var flag = GameObject.Find("Flag").gameObject;
        // 規定秒数待機
        yield return new WaitForSeconds(waitTime);
        switch (cameraMoveMode)
        {
            case CAMERA_MOVEMODE.TO_START:
                // フェードアウト
                yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEOUT));
                // カメラをゴールフラッグと同じ座標にする
                // ゴールフラッグの位置を開始点にする
                startPos = Camera.main.transform.position;
                startPos.x = flag.transform.position.x;
                Camera.main.transform.position = startPos;
                // フェードイン
                yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEIN));
                // スタート地点を終了点にする
                endPos = new Vector3(trafficLightPosX, 0, -10);
                // 移動方向を計算
                moveVec = endPos - startPos;
                break;
            case CAMERA_MOVEMODE.TO_GOAL:
                // スタート地点を開始点にする
                startPos = new Vector3(trafficLightPosX, 0, -10);
                // ゴールフラッグの位置をを終了点にする
                endPos = Camera.main.transform.position;
                endPos.x = flag.transform.position.x;
                // 移動方向を計算
                moveVec = endPos - startPos;
                break;

        }
        // 正規化
        Vector3 normalVec = moveVec.normalized;
        // スタート地点からゴールフラッグまでの距離を測る
        float distance = Vector3.Distance(startPos, endPos);
        var moveValuePer1SecondSpeed = distance / moveTime;
        float nowDistance = 0;
        while(true)
        {
            // スタート地点とゴールフラッグから1フレームでの移動量を計算
            var moveValuePer1FrameSpeed = moveValuePer1SecondSpeed * Time.deltaTime;
            // 移動
            Camera.main.transform.position += normalVec * moveValuePer1FrameSpeed;
            // 現在の距離を更新
            nowDistance += moveValuePer1FrameSpeed;
            // 規定距離以上進んだら終了
            if(nowDistance>=distance)
            {
                // 位置修正
                Camera.main.transform.position = endPos;
                // ゴールへ行くモードなら
                if(cameraMoveMode==CAMERA_MOVEMODE.TO_GOAL)
                {
                    yield return new WaitForSeconds(waitTime);
                    // フェードアウト
                    yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEOUT));
                    // スタート地点へ移動
                    Camera.main.transform.position = new Vector3(trafficLightPosX+startCameraPosOffset, 0, -10);
                    // フェードイン
                    yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEIN));
                }
                yield break;

            }
            if(GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any)||
                Input.GetKeyDown(KeyCode.Return))
            {
                // フェードアウト
                yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEOUT));
                // スタート地点へ移動
                Camera.main.transform.position = new Vector3(trafficLightPosX + startCameraPosOffset, 0, -10);
                // フェードイン
                yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEIN));
                yield break;
            }
            yield return null;
        }
    }

}
