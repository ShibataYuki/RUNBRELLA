using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSlideState : IState
{
    // プレイヤーのコンポーネントスクリプト
    PlayerAfterSlide afterSlide;    
    public void Entry(int ID)
    {
        afterSlide = SceneController.Instance.playerEntityData.playerAfterSlides[ID];        
        // ジャンプ受付時間タイマー開始
        afterSlide.StartTimer(ID);
    }

    public void Do(int ID)
    {       
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[ID].Jump();
            return;
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
            return;
        }
       
        //　手すりの当たり判定チェック
        SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
        var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].RayHit;
        var colliderHit = SceneController.Instance.playerEntityData.playerSlides[ID].IsColliderHit;
        // 手すりにヒットしていたら
        if (colliderHit == true || raycastHit == true)
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);
            return;
        }
                           
        // ショットボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerAttacks[ID].Attack();
        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[ID].IsHit == true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, ID);
        }

        if (InputManager.Instance.BoostKeyHold(ID))
        {
            SceneController.Instance.playerEntityData.playerCharges[ID].Charge();
        }
        else if (InputManager.Instance.BoostKeyOut(ID))
        {
            if (SceneController.Instance.playerEntityData.playerCharges[ID].BoostCheck())
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, ID);
            }
        }

    }

    public void Do_Fix(int ID)
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerAerial[ID].Aerial();
    }
    
    public void Exit(int ID)
    {
        // ジャンプ受付時間タイマーを止める
        afterSlide.StopTimer();        
        // 演出の終了
        SceneController.Instance.playerEntityData.playerSlides[ID].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[ID].EffectOff();
    }
   
}
