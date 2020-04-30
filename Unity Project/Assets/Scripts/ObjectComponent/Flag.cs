using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{

    bool isHit = false;
    // ゴール時の音
    [SerializeField]
    AudioClip audioClip = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突相手がプレイヤーなら
        if(collision.tag=="Player")
        {
            // プレイヤーを順位順のリストに格納
            SceneController.Instance.InsertGoalPlayer(collision.gameObject);
            // 終了処理
            if(!isHit&&SceneController.Instance.isStart)
            {
                // 音再生
                AudioManager.Instance.PlaySE(audioClip, 0.5f);
                // ゴール時用ニュース演出開始
                // UIManager.Instance.newsUIManager.ShowNewsUI(NEWSMODE.GOAL);
                // 終了処理開始
                SceneController.Instance.StartEnd(collision.gameObject);
                // 旗に触れたフラグをONにする
                SceneController.Instance.isTouchFlag = true;
            }
            isHit = true;
        }
    }
}
