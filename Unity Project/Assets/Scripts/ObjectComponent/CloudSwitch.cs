using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSwitch : MonoBehaviour
{
    // 使用済みチェック
    bool isUsed = false;
    //Cloud cloud;
    VerticalRain verticalRain;

    private void Start()
    {         
        //cloud = GameObject.Find("Cloud").GetComponent<Cloud>();
        verticalRain = GameObject.Find("Main Camera/Rain").GetComponent<VerticalRain>();
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
            //if(cloud.mode == Cloud.Mode.IDlE)
            //{
            //    cloud.SetCloud();
            //    isUsed = true;
            //}

            // 雨の呼び出し
            if (verticalRain.mode == VerticalRain.RainMode.IDLE)
            {
                verticalRain.StartRain();
                isUsed = true;
            }

        }
    }
}
