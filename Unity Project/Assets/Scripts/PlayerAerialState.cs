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
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            // 滑空状態に移行
        }
        // 着地したら
        if (SceneManager.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

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

        // ショットボタンが押されたら
        if(InputManager.Instance.ShotKeyIn(ID))
        {
            SceneManager.Instance.playerEntityData.playerShots[ID].
                Shot(SceneManager.Instance.playerObjects[ID].transform.position);
        }

    }

    public void Do_Fix(int ID)
    {
        
    }


    public void Exit(int ID)
    {
        
    }
}
