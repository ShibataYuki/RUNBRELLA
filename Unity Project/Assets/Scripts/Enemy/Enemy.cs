using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    #region ステート変数
    private IdleState idleState;
    private RunState runState;
    private AerialState aerialState;
    private GlideState glideState;
    private SlideState slideState;
    private AfterSlideState afterSlideState;
    private BoostState boostState;
    private DownState downState;
    #endregion

    // ジャンプするシチュエーションかチェックするコンポーネント
    EnemyJumpCheck jumpCheck;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // ステートのコンポーネントを取得
        idleState = GetComponent<IdleState>();
        runState = GetComponent<RunState>();
        aerialState = GetComponent<AerialState>();
        glideState = GetComponent<GlideState>();
        slideState = GetComponent<SlideState>();
        afterSlideState = GetComponent<AfterSlideState>();
        boostState = GetComponent<BoostState>();
        downState = GetComponent<DownState>();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Start()
    {
        base.Start();
        jumpCheck = GetComponent<EnemyJumpCheck>();
    }

    #region ステートの変更
    public override void AerialStart()
    {
        ChangeState(aerialState);
    }

    public override void AfterSlideStart()
    {
        ChangeState(afterSlideState);
    }

    public override void BoostStart()
    {
        ChangeState(boostState);
    }

    public override void Down()
    {
        ChangeState(downState);
    }

    public override void GlideStart()
    {
        ChangeState(glideState);
    }

    public override void IdleStart()
    {
        ChangeState(idleState);
    }

    public override void RunStart()
    {
        ChangeState(runState);
    }

    public override void SlideStart()
    {
        ChangeState(slideState);
    }

    public override void AfterGoalStart()
    {
    }
    #endregion
    #region 現在のステートを確認するためのget
    public override bool IsIdle { get { return state == idleState; } }

    public override bool IsRun { get { return state == runState; } }

    public override bool IsAerial { get { return state == aerialState; } }

    public override bool IsGlide { get { return state == glideState; } }

    public override bool IsSlide { get { return state == slideState; } }

    public override bool IsAfterSlide { get { return state == afterSlideState; } }

    public override bool IsBoost { get { return state == boostState; } }

    public override bool IsDown { get { return state == downState; } }

    public override bool IsAfterGoal { get; }
    #endregion
    #region 特定のアクションを行うか
    /// <summary>
    /// ジャンプするシチュエーションかどうか
    /// </summary>
    public override bool IsJumpStart { get { return jumpCheck.JumpCheck(); } }
    /// <summary>
    /// 滑空するシチュエーションか
    /// </summary>
    public override bool IsGlideStart => throw new System.NotImplementedException();

    /// <summary>
    /// 滑空を終了するシチュエーションか
    /// </summary>
    public override bool IsGlideEnd => throw new System.NotImplementedException();

    /// <summary>
    /// 手すりを掴むシチュエーションか
    /// </summary>
    public override bool IsSlideStart => throw new System.NotImplementedException();

    /// <summary>
    /// 手すりを離すシチュエーションか
    /// </summary>
    public override bool IsSlideEnd => throw new System.NotImplementedException();

    /// <summary>
    /// 攻撃するシチュエーションか
    /// </summary>
    public override bool IsAttack => throw new System.NotImplementedException();

    /// <summary>
    /// チャージするシチュエーションか
    /// </summary>
    public override bool IsCharge => throw new System.NotImplementedException();

    /// <summary>
    /// ブーストするシチュエーションか
    /// </summary>
    public override bool IsBoostStart => throw new System.NotImplementedException();
    #endregion
}