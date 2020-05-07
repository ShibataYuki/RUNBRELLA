using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    enum PlayerMoveDirection
    {
        RIGHT,
        LEFT,
    }

    PlayerMoveDirection playerMoveDirection;

    // 動かすカメラ
    new Camera camera;
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
        // 初期化
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerMoveDirection = PlayerMoveDirection.RIGHT;
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            playerXPosOrder.Add(SceneController.Instance.playerObjects[controllerNo]);
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
        if(pos-camera.transform.position.x>moveOffset)
        {
            // カメラの位置を取得
            Vector3 cameraPos = camera.transform.position;
            cameraPos.x = pos - moveOffset;
            // cameraPos.y = pos.y;
            // カメラの位置を修正
            camera.transform.position = cameraPos;
        }
    }


    public IEnumerator MoveCameraProduction()
    {
        // カメラをゴールフラッグと同じ座標にする
        var flag = GameObject.Find("Flag").gameObject;
        Vector3 goalPos = Camera.main.transform.position;
        goalPos.x = flag.transform.position.x;
        Camera.main.transform.position = goalPos;
        // スタート地点を設定
        Vector3 startPos = new Vector3(0, 0, -10);
        // 移動方向を計算
        Vector3 moveVec = startPos - goalPos;
        // 正規化
        Vector3 normalVec = moveVec.normalized;
        // スタート地点からゴールフラッグまでの距離を測る
        float distance = Vector3.Distance(startPos, goalPos);
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
                Camera.main.transform.position = startPos;
                yield break;
            }
            yield return null;
        }
    }

}
