using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Load
{
    public class SceneController : MonoBehaviour
    {        
        // Update is called once per frame
        void Update()
        {
            // スプレッドシートからのデータ読み込み完了を待ってシーン移行
            if(SheetToDictionary.Instance.IsCompletedSheetToText)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}

