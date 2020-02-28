using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(int ID)
    {
        SceneManager.Instance.playerEntityData.playerRuns[ID].Run();
    }

    public void Do(int ID)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {

            //　ジャンプ
            SceneManager.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }
    }

    public void Do_Fix(int ID)
    {

    }

    public void Exit(int ID)
    {
    }
}
