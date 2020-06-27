using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppers : MonoBehaviour
{
    // 子のすべての紙吹雪エフェクトを格納するリスト
    [SerializeField]
    List<GameObject> popperList = new List<GameObject>();
    [SerializeField]
    AudioClip se = null;
    // Start is called before the first frame update
    
    /// <summary>
    /// すべての紙吹雪エフェクトを再生する
    /// </summary>
    public void PlayPoperEffect()
    {
        // リスト内のすべての紙吹雪エフェクトアクティブにする
        // （アクティブにするとPopper.csで再生される）
        foreach(GameObject popperObj in popperList)
        {
            popperObj.SetActive(true);           
        }
        AudioManager.Instance.PlaySE(se,0.5f);
    }

}
