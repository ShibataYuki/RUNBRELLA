using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : IState
{

    public void Entry(int ID)
    {
        // プレイヤーの移動ベクトルを0にする
        Rigidbody2D rigidbody2d = SceneController.Instance.playerObjects[ID].GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(0, 0);
        // プレイヤーを遅くする
        SceneController.Instance.playerEntityData.playerRuns[ID].SetSpeed(SceneController.Instance.playerEntityData.playerRuns[ID].downSpeed);

        // ボタンを表示
        SceneController.Instance.playerObjects[ID].transform.
            Find("WhenPlayerDown").gameObject.SetActive(true);
        // ボタンを押すアニメーションを開始
        SceneController.Instance.playerObjects[ID].transform.
            Find("WhenPlayerDown").GetComponent<PushButton>().StartPushButtonAnimetion();
    }

    public void Do(int ID)
    {
        // ジャンプボタンを押したらダウン時間を短くする
        if(InputManager.Instance.JumpKeyIn(ID))
        {
            SceneController.Instance.playerEntityData.playerDowns[ID].nowTime += 
                SceneController.Instance.playerEntityData.playerDowns[ID].addTime;
        }
        // 一定時間経過したらダウン状態解除
        if (SceneController.Instance.playerEntityData.playerDowns[ID].
            TimeCounter(SceneController.Instance.playerEntityData.players[ID].downTime))
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, ID);
        }
    }

    public void Do_Fix(int ID)
    {
        SceneController.Instance.playerEntityData.playerRuns[ID].Run();
    }


    public void Exit(int ID)
    {
        // ボタンを非表示
        SceneController.Instance.playerObjects[ID].transform.
            Find("WhenPlayerDown").gameObject.SetActive(false);
        // ボタンを押すアニメーションを終了
        SceneController.Instance.playerObjects[ID].transform.
            Find("WhenPlayerDown").GetComponent<PushButton>().EndPushButtonAnimetion();
        // プレイヤーの速さを戻す
        SceneController.Instance.playerEntityData.playerRuns[ID].SetSpeed(SceneController.Instance.playerEntityData.playerRuns[ID].defaultSpeed);
        // 被弾フラグを解除
        SceneController.Instance.playerEntityData.players[ID].IsHitBullet = false;
    }
}
