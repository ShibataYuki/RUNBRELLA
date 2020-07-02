using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlideState : CharacterState
{
    // 必要なコンポーネント
    protected HitChecker hitChecker;
    protected PlayerJump playerJump;
    protected PlayerAttack playerAttack;
    protected PlayerSlide playerSlide;
    protected PlayerCharge playerCharge;
    protected Character character;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントの取得
        hitChecker = GetComponent<HitChecker>();
        playerJump = GetComponent<PlayerJump>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSlide = GetComponent<PlayerSlide>();
        playerCharge = GetComponent<PlayerCharge>();
    }
    /// <summary>
    /// ステート開始処理
    /// </summary>
    public override void Entry()
    {
        // 滑走の開始処理
        playerSlide.StartSlide();
        playerCharge.ChargeStop();
    }
    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public override void Do()
    {
        // 手すりの上にいるかのチェック処理
        playerSlide.SlideCheck();

        // アクションボタンが押されたら
        if (character.IsSlideEnd == true)
        {
            // y方向への慣性制限
            playerSlide.LimitInertiaY();
            // 空中状態に移行
            character.RunStart();
        } // if(character.IsSlideEnd)

        //　ジャンプボタンが押されたら
        if (character.IsJumpStart == true)
        {
            // y方向への慣性制限
            playerSlide.LimitInertiaY();
            //　ジャンプ
            playerJump.Jump();
            // 空中状態に移行
            character.AerialStart();
        } // if(character.IsJumpStart)

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // キャラの傾きを戻す
            transform.rotation = Quaternion.identity;
            // y方向への慣性制限
            playerSlide.LimitInertiaY();
            // ダウン状態に移行
            character.Down();
        }
        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();

    } // Do

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    public override void Do_Fix()
    {
        // 手すりから離れたら
        var rayHit = playerSlide.RayHit;
        if (rayHit == false)
        {            
            // y方向への慣性制限
            playerSlide.LimitInertiaY();
            // 手すり後のジャンプ猶予状態に移行
            character.AfterSlideStart();
        }
        // 滑走処理
        playerSlide.Slide();
        // 接地判定
        hitChecker.GroundCheckSlider();
        //// 地面についたら
        if (character.IsGround == true)
        {
            // キャラの傾きを戻す
            transform.rotation = Quaternion.identity;
            // ラン状態に移行
            character.RunStart();
        }
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // 滑走の終了処理
        playerSlide.EndSlide();
    }
}