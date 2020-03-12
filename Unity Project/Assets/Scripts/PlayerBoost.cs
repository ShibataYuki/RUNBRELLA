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

    // ブースト中の重力の大きさ
    [SerializeField]
    private float boostGravityScale = 0.0f;
    // ブースト前の重力の大きさ
    private float defaultGravityScale;

	// 地面のチェックするためのクラス
    private PlayerAerial aerial;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
        aerial = GetComponent<PlayerAerial>();
    }

    /// <summary>
    /// エネルギーをチェックしてブーストの開始処理
    /// </summary>
    /// <param name="ID">チェックするプレイヤーのID</param>
    public void BoostStart(int ID)
    {
        // エネルギーが3以上なら
        if (SceneController.Instance.playerEntityData.playerShots[ID].nowBulletCount >=3)
        {
            // エネルギーを-3する。
            SceneController.Instance.playerEntityData.playerShots[ID].nowBulletCount -= 3;
            // ステートをブーストに変更
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, ID);
            // ブースト中の重力を使用する
            BoostGravityStart();
        }
    }

    /// <summary>
    /// ブースト中の重力を使用する
    /// </summary>
    public void BoostGravityStart()
    {
        defaultGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = boostGravityScale;
    }

    /// <summary>
    /// ブースト処理
    /// </summary>
    public void Boost()
    {
        // 前方にブロックがあるなら
        if(aerial.FrontGroundCheck() == true)
        {
            // X方向のベロシティを0にする。
            rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
        }
        else
        {
            // 距離ベクトルを計算して、力を加える
            rigidbody.velocity = new Vector2(boostSpeed, rigidbody.velocity.y);
        }
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
            ReturnGravityScale();
            return true;
        }
        else
        {
            return false; 
        }
    }

    /// <summary>
    /// 重力をブーストする以前の重力に変更
    /// </summary>
    public void ReturnGravityScale()
    {
        // 重力をもとに戻す
        rigidbody.gravityScale = defaultGravityScale;
    }
}
