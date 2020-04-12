using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{

    bool isHit = false;

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
            if(!isHit&&!SceneController.Instance.isEnd)
            {
                SceneController.Instance.StartEnd(collision.gameObject);
            }
            isHit = true;
        }
    }
}
