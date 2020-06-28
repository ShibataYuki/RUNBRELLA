using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlideState : CharacterState
{
    // コンポーネント
    protected PlayerGlide playerGlide;
    protected Character character;
    protected AerialState aerialState;
    protected RunState runState;
    protected DownState downState;
    protected PlayerCharge playerCharge;
    protected PlayerAttack playerAttack;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Start()
    {
        // コンポーネントを取得
        playerGlide = GetComponent<PlayerGlide>();
        character = GetComponent<Character>();
        aerialState = GetComponent<AerialState>();
        runState = GetComponent<RunState>();
        downState = GetComponent<DownState>();
        playerCharge = GetComponent<PlayerCharge>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    public override void Entry()
    {
        // 滑空開始処理
        playerGlide.StartGlide();
    }

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    public override void Do()
    {
        // ジャンプボタンが離されたら
        if (character.IsGlideEnd == true)
        {
            // 空中状態に移行
            character.AerialStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
        }

        // 地面についたら
        if (character.IsGround == true)
        {
            // ラン状態に移行
            character.RunStart();
            // ブーストのキー入力を確認
            playerCharge.BoostKeyCheck();
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
    public override void Do_Fix()
    {       
        // 滑空中処理
        playerGlide.Gride();       
    }
    
    /// <summary>
    /// ステートの終了処理
    /// </summary>
    public override void Exit()
    {
        // 滑空終了処理
        playerGlide.EndGlide();
    }
}
