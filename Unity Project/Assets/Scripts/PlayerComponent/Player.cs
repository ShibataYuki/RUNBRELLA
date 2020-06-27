using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    #region ステート変数
    private PlayerAerialState aerialState;
    private PlayerAfterSlideState afterSlideState;
    private PlayerBoostState boostState;
    private PlayerDownState downState;
    private PlayerGlideState glideState;
    private PlayerIdleState idleState;
    private PlayerRunState runState;
    private PlayerSlideState slideState;
    private PlayerAfterGoalState afterGoalState;
    #endregion
    // プレイヤーのコントローラナンバー
    public CONTROLLER_NO controllerNo { protected get; set; } = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // アタッチされているステートを取得
        aerialState = GetComponent<PlayerAerialState>();
        afterSlideState = GetComponent<PlayerAfterSlideState>();
        boostState = GetComponent<PlayerBoostState>();
        downState = GetComponent<PlayerDownState>();
        glideState = GetComponent<PlayerGlideState>();
        idleState = GetComponent<PlayerIdleState>();
        runState = GetComponent<PlayerRunState>();
        slideState = GetComponent<PlayerSlideState>();
        afterGoalState = GetComponent<PlayerAfterGoalState>();
    }
    #region ステートを変更するためのアクセサーメソッド
    public override void IdleStart()
    {
        ChangeState(idleState);
    }
    public override void RunStart()
    {
        ChangeState(runState);
    }
    public override void AerialStart()
    {
        ChangeState(aerialState);
    }
    public override void GlideStart()
    {
        ChangeState(glideState);
    }
    public override void SlideStart()
    {
        ChangeState(slideState);
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
    public override void AfterGoalStart()
    {
        ChangeState(afterGoalState);
    }
    #endregion
    #region 現在のステートを確認するためのget
    public override bool IsIdle       { get { return state ==       idleState; } }
    public override bool IsRun        { get { return state ==        runState; } }
    public override bool IsAerial     { get { return state ==     aerialState; } }
    public override bool IsGlide      { get { return state ==      glideState; } }
    public override bool IsSlide      { get { return state ==      slideState; } }
    public override bool IsAfterSlide { get { return state == afterSlideState; } }
    public override bool IsBoost      { get { return state ==      boostState; } }
    public override bool IsDown       { get { return state ==       downState; } }
    public override bool IsAfterGoal  { get { return state ==  afterGoalState; } }
    #endregion
    #region 特定のアクションを行うか
    public override bool IsJumpStart  { get { return InputManager.Instance.JumpKeyIn(controllerNo); } }
    public override bool IsGlideStart { get { return InputManager.Instance.StartGlidingKeyIn(controllerNo); } }
    public override bool IsGlideEnd   { get { return InputManager.Instance.EndGlidingKeyIn(controllerNo); } }
    public override bool IsSlideStart { get { return InputManager.Instance.ActionKeyIn(controllerNo); } }
    public override bool IsSlideEnd   { get { return InputManager.Instance.ActionKeyIn(controllerNo); } }
    public override bool IsAttack     { get; /*{ return InputManager.Instance.AttackKeyIn(controllerNo); } */}
    public override bool IsCharge     { get { return InputManager.Instance.ShotAndBoostKeyIn(controllerNo); } }
    public override bool IsBoostStart { get { return InputManager.Instance.ShotAndBoostKeyOut(controllerNo); } }
    #endregion
}
