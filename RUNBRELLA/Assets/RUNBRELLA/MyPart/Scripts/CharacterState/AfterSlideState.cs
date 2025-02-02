﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AfterSlideState : MonoBehaviour,I_CharacterState
{
    // 必要なコンポーネント
    protected PlayerJump playerJump;
    protected PlayerCharge playerCharge;
    protected PlayerAfterSlide playerAfterSlide;
    protected PlayerAttack playerAttack;
    protected PlayerAerial playerAerial;
    protected PlayerSlide playerSlide;
    protected Character character;
    protected Rigidbody2D playerRigidbody2D;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントを取得
        playerJump = GetComponent<PlayerJump>();
        playerCharge = GetComponent<PlayerCharge>();
        playerAfterSlide = GetComponent<PlayerAfterSlide>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAerial = GetComponent<PlayerAerial>();
        playerSlide = GetComponent<PlayerSlide>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    public virtual void Entry()
    {
        // ジャンプ受付時間タイマー開始
        playerAfterSlide.ExitStateTimer_Start();
        // 重力加速度を変更
        playerRigidbody2D.gravityScale = 1f;
        // スライド後状態用の手すりをつかむ猶予時間をセット
        var catchSliderTime = playerAfterSlide.catchSliderTime_SlideToSlide;
        // 手すりヒット判定を発生させる
        playerSlide.CatchTimerStart(catchSliderTime);
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public virtual void Do()
    {
        //　ジャンプボタンが押されたら
        if (character.IsJumpStart == true)
        {
            // 空中状態に移行
            character.AerialStart();
            //　ジャンプ
            playerJump.Jump();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
            return;
        }
        // 着地したら
        if (character.IsGround == true)
        {
            // ラン状態に移行
            character.RunStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
            return;
        }

        // ショットボタンが押されたら
        if (character.IsAttack == true)
        {
            // 攻撃関数呼び出し
            playerAttack.Attack();
        }

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に移行
            character.Down();
        }
        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();
    }

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    public virtual void Do_Fix()
    {
        // 空中状態と同様の挙動をさせる
        playerAerial.Aerial();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public virtual void Exit()
    {
        // ジャンプ受付時間タイマーを止める
        playerAfterSlide.StopTimer();        
    }
}
