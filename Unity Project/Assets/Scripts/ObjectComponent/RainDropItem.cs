using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropItem : MonoBehaviour
{

    // 画面外かどうかのフラグ
    bool isScreen = true;
    RainDropItemFactory rainDropItemFactory;

    // Start is called before the first frame update
    void Start()
    {
        rainDropItemFactory = GameObject.Find("RainDropItemFactory").GetComponent<RainDropItemFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScreen)
        {
            // 画面外ならプールに戻す
            rainDropItemFactory.ReturnItem(gameObject);
        }
        isScreen = false;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // アイテム取得時処理
            // アイテムをプールに戻す
            rainDropItemFactory.ReturnItem(gameObject);
        }

    }
    /// <summary>
    /// メインカメラ内にいるか判定する関数
    /// </summary>
    private void OnWillRenderObject()
    {
        if (Camera.current.name == "Main Camera")
        {
            isScreen = true;
        }
    }

}
