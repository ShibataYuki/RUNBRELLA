using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{
    
    public void Entry(CONTROLLER_NO controllerNo)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].StartAerial();
    }

    public void Entry(CONTROLLER_NO controllerNo, RaycastHit2D hit)
    {
    }

    public void Do(CONTROLLER_NO controllerNo)
    {
        // ジャンプボタンが押されたら
        if (InputManager.Instance.StartGlidingKeyIn(controllerNo))
        {
            // 滑空状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerGlideState, controllerNo);
            // チャージ演出を一時停止する
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].ChargeStop();
            return;
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[controllerNo].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, controllerNo);
            return;
        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(controllerNo))
        {
            // 手すりヒット判定
            SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayTimerStart(0.1f,controllerNo);
            // 演出の終了
            SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOff();
            // チャージ演出を一時停止する
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].ChargeStop();
        }
        else
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[controllerNo].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].RayHit;
            var colliderHit = SceneController.Instance.playerEntityData.playerSlides[controllerNo].IsColliderHit;
            // 手すりにヒットしていたら
            if (colliderHit == true || raycastHit ==true)
            {
                // 掴める演出
                SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOn();
            }
            else
            {
                // もうすぐ掴めるかチェックして掴めそうならエフェクトを少し付ける
                SceneController.Instance.playerEntityData.playerSlides[controllerNo].SliderCheckSoon();  
            }

            // 上昇気流内にいるかチェック
            SceneController.Instance.playerEntityData.playerAerial[controllerNo].UpdraftCheck();
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

        if (InputManager.Instance.BoostKeyHold(controllerNo))
        {
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].Charge();
        }
        else if (InputManager.Instance.BoostKeyOut(controllerNo))
        {
            if (SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostCheck())
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, controllerNo);
            }
        }
    }

    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].Aerial();        
    }


    public void Exit(CONTROLLER_NO controllerNo)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].EndAerial();
        // 演出の終了
        SceneController.Instance.playerEntityData.playerSlides[controllerNo].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[controllerNo].EffectOff();
    }
}
