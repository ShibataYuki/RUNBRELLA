using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : IState
{
    public void Entry(int ID)
    {
        // 滑走の開始処理
        SceneController.Instance.playerEntityData.playerSlides[ID].StartSlide();       
    }

    public void Do(int ID)
    {
        // 手すりの上にいるかのチェック処理
        SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
        

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            var rigidBody = SceneController.Instance.playerEntityData.players[ID].Rigidbody;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }
        
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            var rigidBody = SceneController.Instance.playerEntityData.players[ID].Rigidbody;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.players[ID].IsHitBullet == true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, ID);
        }


    }

    public void Do_Fix(int ID)
    {
        // 手すりから離れたら
        var rayHit = SceneController.Instance.playerEntityData.playerSlides[ID].RayHit;
        var colliderHit = SceneController.Instance.playerEntityData.playerSlides[ID].IsColliderHit;
        if (colliderHit == false && rayHit == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }
        // 滑走処理
        SceneController.Instance.playerEntityData.playerSlides[ID].Slide();
        //// 地面についたら
        //if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        //{
        //    // ラン状態に移行
        //    PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

        //}        
    }

    public void Exit(int ID)
    {
        // 滑走の終了処理
        SceneController.Instance.playerEntityData.playerSlides[ID].EndSlide();
    }    
}
