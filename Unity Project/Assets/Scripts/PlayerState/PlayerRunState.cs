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
            SceneController.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            // 手すりヒット判定
            SceneController.Instance.playerEntityData.playerSlides[ID].RayTimerStart(0.1f, ID);
        }
        else
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].RayHit;
            var colliderHit = SceneController.Instance.playerEntityData.playerSlides[ID].IsColliderHit;
            // 手すりにヒットしていたら
            if (colliderHit == true || raycastHit == true)
            {
                // エフェクトをONにする
                SceneController.Instance.playerEntityData.playerSlides[ID].EffectOn();
            }
            else
            {
                // エフェクトを少し加えるかチェックする
                SceneController.Instance.playerEntityData.playerSlides[ID].SliderCheckSoon();
            }

            SceneController.Instance.playerEntityData.playerAerial[ID].UpdraftCheck();
        }

        // 地面から落ちたら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[ID].IsHit==true)
        {
            // ダウン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerDownState, ID);
        }

        // アタックボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerAttacks[ID].Attack();
        }

        if (InputManager.Instance.BoostKeyHold(ID))
        {
            SceneController.Instance.playerEntityData.playerCharges[ID].Charge();
        }
        else if(InputManager.Instance.BoostKeyOut(ID))
        {
            if (SceneController.Instance.playerEntityData.playerCharges[ID].BoostCheck())
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, ID);
            }
        }

    }

    public void Do_Fix(int ID)
    {
        SceneController.Instance.playerEntityData.playerRuns[ID].Run();
    }

    public void Exit(int ID)
    {
        // エフェクトをOFFにする
        SceneController.Instance.playerEntityData.playerSlides[ID].EffectOff();
        SceneController.Instance.playerEntityData.playerAerial[ID].EffectOff();
    }
}
