using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(int ID)
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

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            SceneManager.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneManager.Instance.playerEntityData.playerSlides[ID].Hit;

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {                
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);

            }
        }

        // 地面から落ちたら
        if (SceneManager.Instance.playerEntityData.players[ID].IsGround == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
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
