using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{   
    public void Entry(int ID)
    {
        // デバッグ用色変更
        var sprite = SceneManager.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
    }
   

    public void Do(int ID)
    {
        //　ジャンプボタンが押されたら
        if (InputManager.Instance.JumpKeyIn(ID))
        {
            //Debug.Log(SceneManager.Instance.playerEntityData.players[ID].IsGround);
            //　ジャンプ
            SceneManager.Instance.playerEntityData.playerJumps[ID].Jump();
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);

        }

        // アクションボタンが押されたら
        if (InputManager.Instance.ActionKeyIn(ID))
        {
            //　手すりの当たり判定チェック
            SceneManager.Instance.playerEntityData.playerSlides[ID].SlideCheck();
            var raycastHit = SceneManager.Instance.playerEntityData.playerSlides[ID].Hit;

            // 手すりにヒットしていたら
            if (raycastHit == true)
            {                
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerSlideState, ID);

            }
        }

        // 地面から落ちたら
        if (SceneManager.Instance.playerEntityData.players[ID].IsGround == false)
        {
            // 空中状態に移行
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerAerialState, ID);
        }

        // ショットボタンが押されたら
        if (InputManager.Instance.ShotKeyIn(ID))
        {
            SceneManager.Instance.playerEntityData.playerShots[ID].
                Shot(SceneManager.Instance.playerObjects[ID].transform.position);
        }


        SceneManager.Instance.playerEntityData.playerRuns[ID].Run();

    }

    public void Do_Fix(int ID)
    {

    }

    public void Exit(int ID)
    {
    }
}
