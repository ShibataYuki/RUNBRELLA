using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPop : MonoBehaviour
{
    // テキスト
    [SerializeField]
    Text text = null;
   
    /// <summary>
    /// ゲームを終了させる処理
    /// </summary>
    public void QuitGame()         // UIでボタンを押したときに呼ばれる
    {
        Debug.Log("aaa");
        UnityEngine.Application.Quit();
    }
  
    public void ChangeText(string text)
    {
        this.text.text = text;
    }
}
