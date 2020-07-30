using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSwitch : MonoBehaviour
{
    // 使用済みチェック
    bool isUsed = false;    
    Rain verticalRain;
    RainIconFactory rainIconFactory;


    private void Start()
    {                
        verticalRain = GameObject.Find("Main Camera/Rain").GetComponent<Rain>();
        rainIconFactory = GameObject.Find("UIManager/RainIconFactory").GetComponent<RainIconFactory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // すでに使った看板ならreturn
        if(isUsed == true)
        {
            return;
        }       
        if(collision.gameObject.tag == "Player")
        {                       
            // 雨を降らせる処理開始
            verticalRain.StartRain();
            // 雨天時のニュース演出開始
            UIManager.Instance.newsUIManager.EntryNewsUI(NEWSMODE.RAIN);
            // 雨のアイコン発生処理
            rainIconFactory.StartRainIcon(transform.position);
            isUsed = true;            
        }
    }
}
