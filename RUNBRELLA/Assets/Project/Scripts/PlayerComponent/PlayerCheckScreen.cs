using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckScreen : MonoBehaviour
{

    // プレイヤーのRenderer
    Renderer playerRenderer;
    // プレイヤーが画面内にいるかどうか
    bool isScreen = false;
    // プレイヤー死亡時のスクリプト
    ShockCamera shockCamera;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        playerRenderer = GetComponent<Renderer>();
        shockCamera = Camera.main.GetComponent<ShockCamera>();
    }

    private void Update()
    {
        if(!isScreen)
        {
            // ゲーム中なら
            if(SceneController.Instance.isStart)
            {
                // 死亡時用ニュース演出開始
                UIManager.Instance.newsUIManager.EntryNewsUI(NEWSMODE.DEAD, gameObject);
                // カメラを揺らす
                shockCamera.StartShock();
            }
            // プレイヤーの順位順のリストに格納
            SceneController.Instance.InsertDeadPlayer(gameObject);
            gameObject.SetActive(false);
        }
        isScreen = false;
    }

    private void OnWillRenderObject()
    {
        if(Camera.current.name=="Main Camera")
        {
            isScreen = true;
        }
    }
}
