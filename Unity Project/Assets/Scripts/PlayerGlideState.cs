using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : IState
{

    public void Entry(int ID)
    {
        // 滑空開始処理
        SceneController.Instance.playerEntityData.playerGlides[ID].StartGlide();
    }
   
    public void Do(int ID)
    {

        // Y方向への速度の制限処理
        SceneController.Instance.playerEntityData.playerGlides[ID].RestrictVectorY();

        // ジャンプボタンが離されたら
        if (InputManager.Instance.EndGlidingKeyIn(ID) == true)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // 地面についたら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
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
        
    }
    
    public void Exit(int ID)
    {
        // 滑空終了処理
        SceneController.Instance.playerEntityData.playerGlides[ID].EndGlide();
    }
}
