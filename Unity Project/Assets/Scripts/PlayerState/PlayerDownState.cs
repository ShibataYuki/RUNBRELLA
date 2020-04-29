using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : IState
{

    public void Entry(CONTROLLER_NO controllerNo)
    {
        SceneController.Instance.playerEntityData.playerDowns[controllerNo].StartDown();
    }

    public void Do(CONTROLLER_NO controllerNo)
    {
        // ジャンプボタンを押したらダウン時間を短くする
        //if (InputManager.Instance.JumpKeyIn(ID))
        //{
        //    SceneController.Instance.playerEntityData.playerDowns[ID].nowTime +=
        //        SceneController.Instance.playerEntityData.playerDowns[ID].addTime;
        //}
        // 一定時間経過したらダウン状態解除
        if (SceneController.Instance.playerEntityData.playerDowns[controllerNo].
            TimeCounter(SceneController.Instance.playerEntityData.players[controllerNo].downTime))
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, controllerNo);
        }
    }

    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        // SceneController.Instance.playerEntityData.playerRuns[ID].Run();
    }


    public void Exit(CONTROLLER_NO controllerNo)
    {
        // 終了処理
        SceneController.Instance.playerEntityData.playerDowns[controllerNo].EndDown();
    }
}
