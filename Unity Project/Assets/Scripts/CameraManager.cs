using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // 動かすカメラ
    new Camera camera;
    // 一位のx座標
    float firstrunkPosX;
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
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // 毎フレーム順位をチェックし一位の座標を返す
        firstrunkPosX = CheckRanking();
        // カメラを動かす処理
        MoveCamera(firstrunkPosX);

    }


    /// <summary>
    /// 一位のx座標を取得する関数
    /// </summary>
    /// <returns></returns>
    float CheckRanking()
    {

        float x = 0;

        for (int i = 1; i <= SceneManager.Instance.playerCount; i++)
        {
            // 一位のx座標を代入
            if(x<SceneManager.Instance.playerObjects[i].transform.position.x)
            {
                x = SceneManager.Instance.playerObjects[i].transform.position.x;
            }
        }

        return x;

    }


    /// <summary>
    /// カメラの位置を修正する関数
    /// </summary>
    /// <param name="x"></param>
    void MoveCamera(float x)
    {
        // カメラとプレイヤーが一定以上離れたらカメラの位置を修正する
        if(x-camera.transform.position.x>moveOffset)
        {
            // カメラの位置を取得
            Vector3 cameraPos = camera.transform.position;
            cameraPos.x = x - moveOffset;
            // カメラの位置を修正
            camera.transform.position = cameraPos;
        }
    }


}
