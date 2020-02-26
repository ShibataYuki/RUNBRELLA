using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour , IStateManager
{
    
    // 
    IState nowState = null;

    // シングルトンインスタンス
    public static PlayerStateManager Instance
    { get { return instance; } }
    private static PlayerStateManager instance = null;

    private void Awake()
    {
        // シングルトンインスタンス
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
                
    }


    /// <summary>
    /// プレイヤーのステートを取得します
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public IState CheckState(int ID)
    {
        if (nowState == null)
        {
            Debug.Log("Stateがnullでーす");            
        }
        return nowState;
    }

    /// <summary>
    /// 状態変更用関数です
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(IState state)
    {
        // 現在のステートの終了処理を呼ぶ
        nowState.Exit();
        // ステートを変更する
        nowState = state;
        // 変更後の開始処理を呼ぶ
        state.Entry();
    }
    
}
