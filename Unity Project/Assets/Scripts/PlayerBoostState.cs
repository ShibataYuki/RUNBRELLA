using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostState : IState
{
    public void Entry(int ID)
    {
        // ブーストエフェクト再生
        var player = SceneController.Instance.playerEntityData.players[ID].GetComponent<Player>();
        player.PlayEffect(player.boostEffect);
        player.Rigidbody.velocity = Vector2.zero;
        // デバッグ用色変更
        var sprite = SceneController.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = new Color(1.0f, 0.48f, 0.08f);
    }

    public void Entry(int ID, RaycastHit2D hit)
    {
    }

    public void Do(int ID)
    {

        // ジャンプ状態の開始
        if (InputManager.Instance.StartGlidingKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerBoosts[ID].BoostJump();
        }

        // ブーストが終了するかチェック
        if (SceneController.Instance.playerEntityData.playerBoosts[ID].FinishCheck())
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.ShotKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerShots[ID].
                Shot(SceneController.Instance.playerObjects[ID].transform.position, ID);
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
        // 加速処理
        SceneController.Instance.playerEntityData.playerBoosts[ID].Boost();
    }


    public void Exit(int ID)
    {
        // ブーストエフェクト停止
        var player = SceneController.Instance.playerEntityData.players[ID].GetComponent<Player>();
        player.StopEffect(player.boostEffect);
    }
}
