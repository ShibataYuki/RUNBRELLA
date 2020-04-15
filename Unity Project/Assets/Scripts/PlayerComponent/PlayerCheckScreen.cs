using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckScreen : MonoBehaviour
{

    // プレイヤーのRenderer
    Renderer playerRenderer;
    // プレイヤーが画面内にいるかどうか
    bool isScreen = false;
    // 死亡時のエフェクト
    DeadEffect deadEffect;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        playerRenderer = GetComponent<Renderer>();
        deadEffect = GameObject.Find("DeadEffect").GetComponent<DeadEffect>();
    }

    private void Update()
    {
        if(!isScreen)
        {
            // ゲーム中なら
            if(SceneController.Instance.isStart)
            {
                // 死亡時のエフェクトを再生
                deadEffect.StartDeadEffect(transform.position);
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
