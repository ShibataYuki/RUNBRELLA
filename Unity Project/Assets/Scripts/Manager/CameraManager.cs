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
        for (int i = 1; i <= GameManager.Instance.playerNumber; i++)
        {
            playerXPosOrder.Add(SceneController.Instance.playerObjects[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneController.Instance.isStart)
        {
            // 毎フレーム順位をチェックし一位の座標を返す
            CheckRanking(playerMoveDirection);
            if(!SceneController.Instance.isEnd)
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


}
