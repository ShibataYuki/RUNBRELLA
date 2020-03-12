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
    // 一位のx座標
    Vector2 firstrunkPos;
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
    }

    // Update is called once per frame
    void Update()
    {
        // 毎フレーム順位をチェックし一位の座標を返す
        firstrunkPos = CheckRanking(playerMoveDirection);
        // カメラを動かす処理
        MoveCamera(firstrunkPos);

    }


    /// <summary>
    /// 一位のx座標を取得する関数
    /// </summary>
    /// <returns></returns>
    Vector2 CheckRanking(PlayerMoveDirection moveDirection)
    {

        Vector2 pos = new Vector2(0, 0);
        
        switch(moveDirection)
        {
            case PlayerMoveDirection.RIGHT:
                for (int i = 1; i <= SceneController.Instance.playerCount; i++)
                {
                    // プレイヤーが表示されていないなら判定しない
                    if (!SceneController.Instance.playerObjects[i].activeInHierarchy)
                    {
                        continue;
                    }
                    if (pos.x < SceneController.Instance.playerObjects[i].transform.position.x)
                    {
                        pos = SceneController.Instance.playerObjects[i].transform.position;
                    }

                }
                break;
            case PlayerMoveDirection.LEFT:
                for (int i = 1; i <= SceneController.Instance.playerCount; i++)
                {
                    // プレイヤーが表示されていないなら判定しない
                    if (!SceneController.Instance.playerObjects[i].activeInHierarchy)
                    {
                        continue;
                    }
                    if (pos.x < SceneController.Instance.playerObjects[i].transform.position.x)
                    {
                        pos = SceneController.Instance.playerObjects[i].transform.position;
                    }

                }
                break;
        }
        return pos;

    }


    /// <summary>
    /// カメラの位置を修正する関数
    /// </summary>
    /// <param name="pos"></param>
    void MoveCamera(Vector2 pos)
    {
        // カメラとプレイヤーが一定以上離れたらカメラの位置を修正する
        if(pos.x-camera.transform.position.x>moveOffset)
        {
            // カメラの位置を取得
            Vector3 cameraPos = camera.transform.position;
            cameraPos.x = pos.x - moveOffset;
            // cameraPos.y = pos.y;
            // カメラの位置を修正
            camera.transform.position = cameraPos;
        }
    }


}
