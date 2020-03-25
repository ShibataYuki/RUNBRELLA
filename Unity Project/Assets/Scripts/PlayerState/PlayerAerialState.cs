using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{
    
    public void Entry(int ID)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerAerial[ID].StartAerial();
    }

    public void Entry(int ID, RaycastHit2D hit)
    {
    }

    public void Do(int ID)
    {
        // ジャンプボタンが押されたら
        if (InputManager.Instance.StartGlidingKeyIn(ID))
        {
            // 滑空状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerGlideState, ID);
            return;
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
            return;
        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
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
            // 演出の終了
            SceneController.Instance.playerEntityData.playerSlides[ID].EffectOff();
        }
        else
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].RayHit;
            var colliderHit = SceneController.Instance.playerEntityData.playerSlides[ID].IsColliderHit;
            // 手すりにヒットしていたら
            if (colliderHit == true || raycastHit ==true)
            {
                // 掴める演出
                SceneController.Instance.playerEntityData.playerSlides[ID].EffectOn();
            }
            else
            {
                // もうすぐ掴めるかチェックして掴めそうならエフェクトを少し付ける
                SceneController.Instance.playerEntityData.playerSlides[ID].SliderCheckSoon();  
            }

            // 上昇気流内にいるかチェック
            SceneController.Instance.playerEntityData.playerAerial[ID].UpdraftCheck();
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
        
        if (InputManager.Instance.BoostKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerBoosts[ID].BoostStart(ID);
        }
    }

    public void Do_Fix(int ID)
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerAerial[ID].Aerial();        
    }


    public void Exit(int ID)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerAerial[ID].EndAerial();
        // 演出の終了
        SceneController.Instance.playerEntityData.playerSlides[ID].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[ID].EffectOff();
    }
}
