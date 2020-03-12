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
        if(collision.tag=="Player"&&!isHit)
        {
            // 終了処理
            SceneController.Instance.StartEnd();
            isHit = true;
        }
    }
}
