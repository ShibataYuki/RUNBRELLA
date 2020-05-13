using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : IState
{

    public void Entry(CONTROLLER_NO controllerNo)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerGlides[controllerNo].StartGlide();
    }
   
    public void Do(CONTROLLER_NO controllerNo)
    {              
        // ジャンプボタンが離されたら
        if (InputManager.Instance.EndGlidingKeyIn(controllerNo) == true)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
            // ブーストのキー入力を確認
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostKeyCheck(controllerNo);
        }

        // 地面についたら
        if (SceneController.Instance.playerEntityData.players[controllerNo].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, controllerNo);
            // ブーストのキー入力を確認
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostKeyCheck(controllerNo);
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
        // 滑空中処理
        SceneController.Instance.playerEntityData.playerGlides[controllerNo].Gride();       
    }
    
    public void Exit(CONTROLLER_NO controllerNo)
    {
        // 滑空終了処理
        SceneController.Instance.playerEntityData.playerGlides[controllerNo].EndGlide();
    }
}
