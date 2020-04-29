using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostState : IState
{
    public void Entry(CONTROLLER_NO controllerNo)
    {
        // ブーストの開始処理
        SceneController.Instance.playerEntityData.playerBoosts[controllerNo].BoostStart();
        // ブーストエフェクト再生
        var player = SceneController.Instance.playerEntityData.players[controllerNo].GetComponent<Player>();
        player.PlayEffect(player.boostEffect);
        player.Rigidbody.velocity = Vector2.zero;
        // 弾消去エリア展開
        SceneController.Instance.playerEntityData.playerBoosts[controllerNo].VanishBulletsArea_ON();
    }

    public void Entry(CONTROLLER_NO controllerNo, RaycastHit2D hit)
    {
    }

    public void Do(CONTROLLER_NO controllerNo)
    {
        // ブーストが終了するかチェック
        if (SceneController.Instance.playerEntityData.playerBoosts[controllerNo].FinishCheck())
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, controllerNo);
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.AttackKeyIn(controllerNo))
        {
            SceneController.Instance.playerEntityData.playerAttacks[controllerNo].Attack();
        }
       
        // 弾に当たったら
        if (SceneController.Instance.playerEntityData.playerAttacks[controllerNo].IsHit == true)
        {
            // 弾に当たった判定をOFFにする。
            SceneController.Instance.playerEntityData.playerAttacks[controllerNo].IsHit = false;
        }
    }

    public void Do_Fix(CONTROLLER_NO controllerNo)
    {
        // 加速処理
        SceneController.Instance.playerEntityData.playerBoosts[controllerNo].Boost();
    }


    public void Exit(CONTROLLER_NO controllerNo)
    {
        // ブーストエフェクト停止
        var player = SceneController.Instance.playerEntityData.players[controllerNo].GetComponent<Player>();
        player.StopEffect(player.boostEffect);
        // 弾消去エリア解消
        SceneController.Instance.playerEntityData.playerBoosts[controllerNo].VanishBulletsArea_OFF();
    }
}
