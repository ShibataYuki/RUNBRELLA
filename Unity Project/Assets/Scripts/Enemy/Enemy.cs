using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private LayerMask bulletLayer;
    protected Vector2 rightTop;
    [SerializeField]
    private Vector2 leftBottom;
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
    protected override void Awake()
    {
        base.Awake();
        idleState = GetComponent<IdleState>();
        runState = GetComponent<RunState>();
        aerialState = GetComponent<AerialState>();
        glideState = GetComponent<GlideState>();
        slideState = GetComponent<SlideState>();
        afterSlideState = GetComponent<AfterSlideState>();
        boostState = GetComponent<BoostState>();
        downState = GetComponent<DownState>();
    }

    protected override void Start()
    {
        base.Start();
        bulletLayer = LayerMask.GetMask("Bullet");
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
    #endregion
    #region 特定のアクションを行うか
    public override bool IsJumpStart => throw new System.NotImplementedException();

    public override bool IsGlideStart => throw new System.NotImplementedException();

    public override bool IsGlideEnd => throw new System.NotImplementedException();

    public override bool IsSlideStart => throw new System.NotImplementedException();

    public override bool IsSlideEnd => throw new System.NotImplementedException();

    public override bool IsAttack => throw new System.NotImplementedException();

    public override bool IsCharge => throw new System.NotImplementedException();

    public override bool IsBoostStart => throw new System.NotImplementedException();
    #endregion

    private bool IsBulletLine()
    {
        var leftBottom = this.leftBottom + (Vector2)transform.position;
        var rightTop = this.rightTop + (Vector2)transform.position;
        return Physics2D.OverlapArea(leftBottom, rightTop, bulletLayer);
    }
}