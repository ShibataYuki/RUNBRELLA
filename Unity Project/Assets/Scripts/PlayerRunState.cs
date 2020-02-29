using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(int ID)
    {
    }
    public void Entry(int ID, RaycastHit2D hit)
    {
    }


    public void Do(int ID)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            //Debug.Log(SceneManager.Instance.playerEntityData.players[ID].IsGround);
            //　ジャンプ
            SceneManager.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }
        if(InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            var raycastHit = SceneManager.Instance.playerEntityData.playerSliderChecks[ID].RayHitCheck();

            if(raycastHit == true)
            {
                Debug.Log("手すり");
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerGlideState, ID);

            }

        }
        SceneManager.Instance.playerEntityData.playerRuns[ID].Run();

    }

    public void Do_Fix(int ID)
    {

    }

    public void Exit(int ID)
    {
    }
}
