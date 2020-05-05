using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIShow : MonoBehaviour
{

    // 表示する時間
    [SerializeField]
    float showTime = 0;
    // 現在の経過時間
    float nowTime = 0;
    // 次のState
    NewsUIExitState exitState;


    /// <summary>
    /// ShowStateのEntry処理をする関数
    /// </summary>
    public void StartShow()
    {
        // 初期化処理
        nowTime = 0;
        exitState = NewsUIManager.Instance.GetComponent<NewsUIExitState>();
    }

    /// <summary>
    /// ShowSateのDo処理をする関数
    /// </summary>
    public void OnShow()
    {
        nowTime += Time.deltaTime;
        if(nowTime>=showTime)
        {
            var ID = gameObject.GetComponent<NewsUI>().ID;
            // ExitStateへ遷移
            NewsUIManager.Instance.ChangeNewsUIState(exitState, ID);
        }
    }


    /// <summary>
    /// ShowStateのExit処理をする関数
    /// </summary>
    public void EndShow()
    {
        // 終了処理
    }
}
