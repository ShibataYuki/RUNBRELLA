using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{

    

    public void Entry(int ID)
    {
        
    }

    public void Entry(int ID, RaycastHit2D hit)
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

        //　手すりの当たり判定チェック
        var raycastHit = SceneManager.Instance.playerEntityData.playerSliderChecks[ID].RayHitCheck();

        if (raycastHit == true)
        {
            Debug.Log("手すり");
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerGlideState, ID);

        }

    }

    public void Do_Fix(int ID)
    {
        
    }


    public void Exit(int ID)
    {
        
    }
}
