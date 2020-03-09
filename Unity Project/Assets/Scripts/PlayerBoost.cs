using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    // ブースト時のスピード
    [SerializeField]
    private float boostSpeed = 30.0f;
    // 必要なコンポーネント
    private new Rigidbody2D rigidbody;
    // ブーストする時間
    [SerializeField]
    private float boostTime = 0.5f;
    // 経過時間
    private float deltaTime = 0.0f;
    // ジャンプ力
    [SerializeField]
    private Vector2 jumpPower = new Vector2(0.0f, 15.0f);

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// エネルギーをチェックしてブーストの開始処理
    /// </summary>
    /// <param name="ID">チェックするプレイヤーのID</param>
    public void BoostStart(int ID)
    {
        // エネルギーがマックスなら
        if (SceneController.Instance.playerEntityData.playerShots[ID].nowBulletCount == SceneController.Instance.playerEntityData.playerShots[ID].BulletCount)
        {
            // エネルギーを0にする。
            SceneController.Instance.playerEntityData.playerShots[ID].nowBulletCount = 0;
            // ステートをブーストに変更
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, ID);
        }
    }


    /// <summary>
    /// ブースト処理
    /// </summary>
    public void Boost()
    {
        // 距離ベクトルを計算して、力を加える
        rigidbody.velocity = new Vector2(boostSpeed, 0.0f) ;
    }

    /// <summary>
    /// 終了するかチェック
    /// </summary>
    /// <returns>終了するかしないか</returns>
    public bool FinishCheck()
    {
        // 経過時間の計測
        deltaTime += Time.deltaTime;
        // 時間が残っているか
        if (deltaTime >= boostTime)
        {
            // 経過時間のリセット
            deltaTime = 0.0f;
            return true;
        }
        else
        {
            return false; 
        }
    }

    /// <summary>
    /// ブースト中のジャンプ処理
    /// </summary>
    public void BoostJump()
    {
        // ジャンプパワーを加える
        rigidbody.AddForce(jumpPower, ForceMode2D.Impulse);
    }
}
