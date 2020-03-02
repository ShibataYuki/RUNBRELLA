using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(int ID)
    {
        // デバッグ用色変更
        var sprite = SceneController.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
    }
   

    public void Do(int ID)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            //Debug.Log(SceneManager.Instance.playerEntityData.players[ID].IsGround);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].Hit;

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {                
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);

            }
        }

        // 地面から落ちたら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.players[ID].IsHitBullet==true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, ID);
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.ShotKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerShots[ID].
                Shot(SceneController.Instance.playerObjects[ID].transform.position);
        }


        SceneController.Instance.playerEntityData.playerRuns[ID].Run();

    }

    public void Do_Fix(int ID)
    {

    }

    public void Exit(int ID)
    {
    }
}
