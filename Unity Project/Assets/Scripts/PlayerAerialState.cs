using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAerialState : IState
{

    float maxSpeedY = 10;
    float maxSpeedX = 10;


    public void Entry(int ID)
    {
        // デバッグ用色変更
        
       
        var sprite = SceneController.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.cyan;
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
        }
        // 着地したら
        if (SceneController.Instance.playerEntityData.players[ID].IsGround == true)
        {
            // ラン状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].Hit;            

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);

            }
            // 演出の終了
            SceneController.Instance.playerEntityData.playerSlides[ID].EffectOff();
        }
        else
        {
            //　手すりの当たり判定チェック
            SceneController.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneController.Instance.playerEntityData.playerSlides[ID].Hit;

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {
                // 掴める演出
                SceneController.Instance.playerEntityData.playerSlides[ID].EffectOn();
            }
            else
            {
                // もうすぐ掴めるかチェックして掴めそうならエフェクトを少し付ける
                SceneController.Instance.playerEntityData.playerSlides[ID].SliderCheckSoon();  
            }
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

        var rigidBody = SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>();

        if (rigidBody.velocity.y > maxSpeedY)
        {
            SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().velocity
                = new Vector2(rigidBody.velocity.x, maxSpeedY);
        }
        if (rigidBody.velocity.x > maxSpeedX)
        {
            SceneController.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().velocity
                = new Vector2(maxSpeedX, rigidBody.velocity.x);
        }

        var ScreenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;        
        if (SceneController.Instance.playerEntityData.players[ID].transform.position.y > ScreenTop &&
            rigidBody.velocity.y > 0)
        {
            
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,0);
        }

        if (InputManager.Instance.BoostKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerBoosts[ID].BoostStart(ID);
        }
    }

    public void Do_Fix(int ID)
    {
        // プレイヤーの速度が最低速度以下だったら最低速度に変更
        SceneController.Instance.playerEntityData.playerSpeedChecks[ID].SpeedCheck();

        // 速度の保存
        SceneController.Instance.playerEntityData.players[ID].SaveVelocity();
    }


    public void Exit(int ID)
    {
        
    }
}
