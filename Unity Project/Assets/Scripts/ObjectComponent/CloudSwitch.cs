using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSwitch : MonoBehaviour
{
    // 使用済みチェック
    bool isUsed = false;
    Cloud cloud;

    private void Start()
    {
        cloud = GameObject.Find("Cloud").GetComponent<Cloud>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isUsed == true)
        {
            return;
        }       
        if(collision.gameObject.tag == "Player")
        {
            if(cloud.mode == Cloud.Mode.IDlE)
            {
                cloud.SetCloud();
                isUsed = true;
            }
            
        }
    }
}
