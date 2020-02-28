using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{

    

    public void Entry(int ID)
    {
        
    }

    public void Do(int ID)
    {
        // ジャンプボタンが押されたら
       if(InputManager.Instance.JumpKeyIn(ID))
        {
            // 滑空状態に移行
        }
        // 着地したら
        if (SceneManager.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

        }

    }

    public void Do_Fix(int ID)
    {
        
    }


    public void Exit(int ID)
    {
        
    }
}
