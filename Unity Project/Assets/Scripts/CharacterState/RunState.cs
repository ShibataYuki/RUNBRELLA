using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunState : CharacterState
{
    // 必要なコンポーネント
    protected PlayerRun playerRun;
    protected PlayerSlide playerSlide;
    protected PlayerAerial playerAerial;
    protected PlayerAttack playerAttack;
    protected PlayerJump playerJump;
    protected PlayerCharge playerCharge;
    protected Character character;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントの取得
        playerRun = GetComponent<PlayerRun>();
        playerSlide = GetComponent<PlayerSlide>();
        playerAerial = GetComponent<PlayerAerial>();
        playerAttack = GetComponent<PlayerAttack>();
        playerJump = GetComponent<PlayerJump>();
        playerCharge = GetComponent<PlayerCharge>();
    }

    public override void Entry()
    {        
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public override void Do()
    {
        //　ジャンプボタンが押されたら
        if (character.IsJumpStart == true)
        {
            //Debug.Log(SceneManager.Instance.playerEntityData.players[ID].IsGround);
            //　ジャンプ
            playerJump.Jump();
            // 空中状態に移行
            character.AerialStart();

        }

        // アクションボタンが押されたら
        if (character.IsSlideStart == true)
        {
            // 手すりをつかむ猶予時間
            var catchSliderTime = playerSlide.catchSliderTime;
            // 手すりヒット判定
            playerSlide.RayTimerStart(catchSliderTime);
        }
        else
        {
            //　手すりの当たり判定チェック
            playerSlide.SlideCheck();
            var raycastHit = playerSlide.RayHit;
            // 手すりにヒットしていたら
            if (raycastHit == true)
            {
                // エフェクトをONにする
                playerSlide.EffectOn();
            }
            else
            {
                // エフェクトをOFFにする
                playerSlide.EffectOff();
            }
            // 上昇気流内にいるかチェック
            playerAerial.UpdraftCheck();
        } // else(character.IsSlideStart)

        // 地面から落ちたら
        if (character.IsGround == false)
        {
            // 空中状態に移行
            character.AerialStart();
        }

        // 弾に当たったら
        if (playerAttack.IsHit == true)
        {
            // ダウン状態に移行
            character.Down();
        }

        // アタックボタンが押されたら
        if (character.IsAttack == true)
        {
            playerAttack.Attack();
        }

        // ブーストのキー入力を確認
        playerCharge.BoostKeyCheck();
    }

    /// <summary>
    /// 物理挙動用のフレーム更新処理
    /// </summary>
    public override void Do_Fix()
    {
        playerRun.Run();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // エフェクトをOFFにする
        playerSlide.EffectOff();
        playerAerial.EffectOff();
    }
}