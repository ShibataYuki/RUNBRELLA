using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : IState
{

    public void Entry(int ID)
    {
        // デバッグ用色変更
        var sprite = SceneManager.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.red;

        // プレイヤーの移動ベクトルを0にする
        Rigidbody2D rigidbody2d = SceneManager.Instance.playerObjects[ID].GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(0, 0);
    }

    public void Do(int ID)
    {
        // 一定時間経過したらダウン状態解除
        if (SceneManager.Instance.TimeCounter(SceneManager.Instance.playerEntityData.players[ID].downTime))
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
        }
    }

    public void Do_Fix(int ID)
    {
        
    }


    public void Exit(int ID)
    {
        // 被弾フラグを解除
        SceneManager.Instance.playerEntityData.players[ID].IsHitBullet = false;
    }
}
