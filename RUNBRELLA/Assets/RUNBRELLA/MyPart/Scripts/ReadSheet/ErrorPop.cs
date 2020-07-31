using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPop : MonoBehaviour
{
    // エラー時に表示されるポップアップウィンドウUI    
    GameObject errorPopObj = null;
    // テキスト    
    Text text = null;

    private void Awake()
    {
        // 非アクティブ状態のオブジェクト検索のために親オブジェクトを取得
        GameObject canvas = GameObject.Find("Canvas");
        // エラーポップのFind
        errorPopObj = canvas.transform.Find("ErrorPop").gameObject;
        // テキストコンポーネントの取得
        text = errorPopObj.transform.Find("Text").GetComponent<Text>();               
    }
    
    /// <summary>
    /// 表示するテキストを変更する
    /// </summary>
    /// <param name="text">表示したい内容</param>
    public void ChangeText(string text)
    {
        this.text.text = text;
    }

    /// <summary>
    /// アクティブ状態にする
    /// </summary>
    public void SetActive()
    {
        errorPopObj.SetActive(true);
    }
}
