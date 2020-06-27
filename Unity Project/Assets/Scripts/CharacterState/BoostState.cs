using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoostState : CharacterState
{
    // 必要なコンポーネント
    protected PlayerBoost playerBoost;
    protected PlayerSlide playerSlide;
    protected PlayerAerialState playerAerialState;
    protected Character character;
    protected PlayerAttack playerAttack;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントの取得
        playerBoost = GetComponent<PlayerBoost>();
        playerSlide = GetComponent<PlayerSlide>();
        playerAerialState = GetComponent<PlayerAerialState>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    public override void Entry()
    {
        // ブーストの開始処理
        playerBoost.BoostStart();
        // ブーストエフェクト再生
        character.PlayEffect(character.boostEffect);
        character.Rigidbody.velocity = Vector2.zero;
        // 弾消去エリア展開
        playerBoost.VanishBulletsArea_ON();
        // キャラの傾きを戻す
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public override void Do()
    {
        // アクションボタンが押されたら
        if (character.IsSlideStart == true)
        {
            // 手すりをつかむ猶予時間
            var catchSliderTime = playerSlide.catchSliderTime;
            // 手すりヒット判定
            playerSlide.RayTimerStart(catchSliderTime);                    
        }
        // ブーストが終了するかチェック
        if (playerBoost.FinishCheck())
        {
            // 地面に接地しているなら
            if (character.IsGround == true)
            {
                // 走り状態に移行
                character.RunStart();
            }
            else
            {
                // 空中状態に移行
                character.AerialStart();
            }
        } // if(playerBoost.FinishCheck)

        // ショットボタンが押されたら
        if (character.IsAttack)
        {
            // 攻撃を行う
            playerAttack.Attack();
        }

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に変更
            character.Down();
        }
    } // Do

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    public override void Do_Fix()
    {
        // 加速処理
        playerBoost.Boost();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // ブーストエフェクト停止
        character.StopEffect(character.boostEffect);
        // 弾消去エリア解消
        playerBoost.VanishBulletsArea_OFF();
    }
}
