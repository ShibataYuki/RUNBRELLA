using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterSlideState : IState
{
    // プレイヤーのコンポーネントスクリプト
    PlayerAfterSlide afterSlide;    
    public void Entry(CONTROLLER_NO controllerNo)
    {
        afterSlide = SceneController.Instance.playerEntityData.playerAfterSlides[controllerNo];        
        // ジャンプ受付時間タイマー開始
        afterSlide.StartTimer(controllerNo);
        // 手すりをつかむ猶予時間
        var catchSliderTime = SceneController.Instance.playerEntityData.playerAfterSlides[controllerNo].catchSliderTime_SlideToSlide;
        // 手すりヒット判定
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayTimerStart(catchSliderTime, controllerNo);
    }

    public void Do(CONTROLLER_NO controllerNo)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(controllerNo))
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
            //　ジャンプ
            SceneController.Instance.playerEntityData.playerJumps[controllerNo].Jump();
            // ブーストのキー入力を確認
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostKeyCheck(controllerNo);
            return;
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[controllerNo].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, controllerNo);
            // ブーストのキー入力を確認
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostKeyCheck(controllerNo);
            return;
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(controllerNo))
        {
            SceneController.Instance.playerEntityData.playerAttacks[controllerNo].Attack();
        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[controllerNo].IsHit == true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, controllerNo);
        }
        // ブーストのキー入力を確認
        SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostKeyCheck(controllerNo);
    }


    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].Aerial();
    }
    
    public void Exit(CONTROLLER_NO controllerNo)
    {
        // ジャンプ受付時間タイマーを止める
        afterSlide.StopTimer();        
        // 演出の終了
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].EffectOff();
    }
   
}
