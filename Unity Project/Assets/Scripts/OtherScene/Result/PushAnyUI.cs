using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushAnyUI : MonoBehaviour
{

    Text text = null;
    IEnumerator alphaPingPong = null;

    /// <summary>
    /// アクティブになった際に行われる処理
    /// </summary>
    private void OnEnable()
    {
        text = GetComponent<Text>();
        alphaPingPong = ChangeAlpha_PingPong();
        StartCoroutine(alphaPingPong);
    }

    /// <summary>
    /// 非アクティブになった際に行われる処理
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(alphaPingPong);
    }

    /// <summary>
    /// α値を0～1の範囲で往復させ、テキストを明滅させる
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeAlpha_PingPong()
    {
        float workAlpha = 0;
        while(true)
        {
            Color newColor = text.color;
            workAlpha = Mathf.PingPong(Time.time, 1);
            newColor.a = workAlpha;
            text.color = newColor;
            yield return null;
        }        
    }

}
