using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : IState
{

    public void Entry(int ID)
    {
        SceneController.Instance.playerEntityData.playerDowns[ID].StartDown();
    }

    public void Do(int ID)
    {
        // ジャンプボタンを押したらダウン時間を短くする
        //if(InputManager.Instance.JumpKeyIn(ID))
        //{
        //    SceneController.Instance.playerEntityData.playerDowns[ID].nowTime += 
        //        SceneController.Instance.playerEntityData.playerDowns[ID].addTime;
        //}
        // 一定時間経過したらダウン状態解除
        //if (SceneController.Instance.playerEntityData.playerDowns[ID].
        //    TimeCounter(SceneController.Instance.playerEntityData.players[ID].downTime))
        //{
        //    PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
        //}
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
    }

    public void Do_Fix(int ID)
    {
        // SceneController.Instance.playerEntityData.playerRuns[ID].Run();
    }


    public void Exit(int ID)
    {
        // 終了処理
        SceneController.Instance.playerEntityData.playerDowns[ID].EndDown();
    }
}
