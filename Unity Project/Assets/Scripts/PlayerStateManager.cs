using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour , IStateManager
{

    // プレイヤーのStateの実体
    public PlayerRunState playerRunState = new PlayerRunState();
    public PlayerAerialState playerAerialState = new PlayerAerialState();
    public PlayerIdelState playerIdelState = new PlayerIdelState();
    public PlayerGlideState playerGlideState = new PlayerGlideState();
    public PlayerSlideState playerSlideState = new PlayerSlideState();
    public PlayerDownState playerDownState = new PlayerDownState();


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

    /// <summary>
    /// 状態変更用関数です
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(IState state, int ID)
    {
        var nowState = SceneController.Instance.playerObjects[ID].GetComponent<Player>().state;
        if (nowState != null)
        {
            // 現在のステートの終了処理を呼ぶ
            nowState.Exit(ID);
        }
        // ステートを変更する
        SceneController.Instance.playerObjects[ID].GetComponent<Player>().state = state;
        // 変更後の開始処理を呼ぶ
        state.Entry(ID);
    }

   
}
