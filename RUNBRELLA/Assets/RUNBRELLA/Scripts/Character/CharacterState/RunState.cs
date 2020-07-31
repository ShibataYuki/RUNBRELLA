using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunState : MonoBehaviour,I_CharacterState
{
    // 必要なコンポーネント
    protected PlayerRun playerRun;
    protected PlayerSlide playerSlide;
    protected PlayerAerial playerAerial;
    protected PlayerAttack playerAttack;
    protected PlayerJump playerJump;
    protected PlayerGlide playerGlide;
    protected PlayerCharge playerCharge;
    protected Character character;
    protected Rigidbody2D playerRigidbody2D;

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
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerGlide = GetComponent<PlayerGlide>();
    }

    public virtual void Entry()
    {
        // 重力を初期化
        playerRigidbody2D.gravityScale = playerAerial.aerialGravityScale;
        // 角度を初期化
        transform.localRotation = Quaternion.identity;
        // 滑空中ホップ可能フラグをtrueにする
        playerGlide.CanHop = true;
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public virtual void Do()
    {
        //　ジャンプボタンが押されたら
        if (character.IsJumpStart == true)
        {            
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
            playerSlide.CatchTimerStart(catchSliderTime);
        }        

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
    public virtual void Do_Fix()
    {
        playerRun.Run();
    }

    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public virtual void Exit()
    {       
    }
}