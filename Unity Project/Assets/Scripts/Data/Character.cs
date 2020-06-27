using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player, Enemyの親クラス(抽象クラス)
/// </summary>
public abstract class Character : MonoBehaviour
{
    // プレイヤーのナンバー
    public PLAYER_NO playerNO { get; set; } = 0;
    // キャラクターのタイプ
    public string Type { get; set; } = "A";
    // 地面にいるか    
    [SerializeField]
    protected bool isGround = false;
    public bool IsGround { set { isGround = value; } get { return isGround; } }
    // プレイヤーステート   
    protected CharacterState state { set; get; } = null;
    // プレイヤーがダウンしている時間
    public float downTime = 0;
    // タイムライン中かどうか
    public bool IsTimeLine { get; set; } = true;

    // 保留
    // プレイヤーの基本の加速度
    //[SerializeField]
    //private float baseAddSpeed = 1.5f;
    //public float BaseAddSpeed { get { return baseAddSpeed; } set { baseAddSpeed = value; } }
    //// 最低スピード
    //[SerializeField]
    //private float baseSpeed = 6;
    //public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }
    //[SerializeField]
    //private float maxSpeed = 10;
    //public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    // 雨に当たっているか
    [SerializeField]
    public bool IsRain = false;
    [SerializeField]
    protected float maxVelocityY = 15f;
    // プレイヤーの速度保存領域
    public float VelocityXStorage { get; set; } = 0;
    // リジッドボディ
    protected Rigidbody2D rigidBody;
    public Rigidbody2D Rigidbody { get { return rigidBody; } }
    protected PlayerSlide playerSlide;
    // アニメーター
    protected Animator animator;

    // AnimatorのパラメーターID
    protected readonly int jumpID = Animator.StringToHash("IsGround");
    protected readonly int runID = Animator.StringToHash("Velocity");
    protected readonly int sliderID = Animator.StringToHash("IsSlider");
    protected readonly int gliderID = Animator.StringToHash("IsGlide");
    protected readonly int downID = Animator.StringToHash("IsDown");
    protected readonly int boostID = Animator.StringToHash("IsBoost");

    // プレイヤーの種類
    public GameManager.CHARTYPE charType;
    // プレイヤーの攻撃手段の種類
    public GameManager.CHARATTACKTYPE charAttackType;

    // 雨を受けているときのエフェクト
    public ParticleSystem feverEffect;
    // ブースト時のエフェクト
    public ParticleSystem boostEffect;
    // チャージ中エフェクト
    public ParticleSystem chargeingEffect;
    // 一段階チャージした際のエフェクト
    public ParticleSystem chargeSignal;
    // チャージが停止中のエフェクト
    public ParticleSystem chargePauseEffect;
    // チャージがMAXの時のエフェクト
    public ParticleSystem chargeMaxEffect;


#if UNITY_EDITOR
    // ステートの名前をデバッグ表示する変数
    [SerializeField]
    protected string stateName;
#endif

    protected virtual void Awake()
    {
        feverEffect = transform.Find("FeverEffect").GetComponent<ParticleSystem>();
        boostEffect = transform.Find("BoostEffect").GetComponent<ParticleSystem>();
        chargeingEffect = transform.Find("ChargeEffects/Charging").GetComponent<ParticleSystem>();
        chargeSignal = transform.Find("ChargeEffects/ChargeSignal").GetComponent<ParticleSystem>();
        chargePauseEffect = transform.Find("ChargeEffects/ChargePause").GetComponent<ParticleSystem>();
        chargeMaxEffect = transform.Find("ChargeEffects/ChargeMax").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();
        ReadTextParameter();
    }

    // Update is called once per frame
    protected void Update()
    {
        // stateのDo関数を呼ぶ
        state.Do();
        Do_AnyState();
#if UNITY_EDITOR
        // 現在のステートをInspecter上に表示
        stateName = state.ToString();
#endif
    }

    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    protected virtual void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var charaATextName = "Chara_A";
        var charaBTextName = "Chara_B";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> charaADictionary;
        Dictionary<string, float> charaBDictionary;
        SheetToDictionary.Instance.TextToDictionary(charaATextName, out charaADictionary);
        SheetToDictionary.Instance.TextToDictionary(charaBTextName, out charaBDictionary);
        try
        {
            // ファイル読み込み
            if (charType == GameManager.CHARTYPE.PlayerA)
            {
                downTime = charaADictionary["プレイヤーのダウンしている時間"];
                // 保留
                //BaseSpeed = gunCharaDictionary["最低速度の秒速"];
                //MaxSpeed = gunCharaDictionary["最高速度の秒速"];
                //BaseAddSpeed = gunCharaDictionary["1秒間に蓄積される加速度"];
            }
            else
            {
                downTime = charaBDictionary["プレイヤーがダウンしている時間"];
                // 保留
                //BaseSpeed = swordCharaDictionary["最低速度の秒速"];
                //MaxSpeed = swordCharaDictionary["最高速度の秒速"];
                //BaseAddSpeed = swordCharaDictionary["1秒間に蓄積される加速度"];
            }
            
        }
        catch
        {
            Debug.Assert(false, nameof(Character) + "でエラーが発生しました");
        }
    }

    /// <summary>
    /// アニメーターにパラメータをセット
    /// </summary>
    protected virtual void SetAnimator()
    {
        // タイムライン中でなければ
        if(IsTimeLine == false)
        {
            animator.SetBool(jumpID, isGround);
            animator.SetBool(sliderID, IsSlide || IsAfterSlide);
            animator.SetFloat(runID, Mathf.Abs(rigidBody.velocity.x));
            animator.SetBool(gliderID, IsGlide);
            animator.SetBool(downID, IsDown);
            animator.SetBool(boostID, IsBoost);
        }
    }

    protected void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        state.Do_Fix();
        Do_Fix_AniState();

    }

    /// <summary>
    /// どのステートでも共通して行う処理です
    /// </summary>
    public void Do_AnyState()
    {
        Do_Rainy();
        // アニメーターにパラメータをセット
        SetAnimator();
    }

    /// <summary>
    /// どのステートでも共通して行う物理処理です
    /// </summary>
    public void Do_Fix_AniState()
    {
        RimitScreenTop();
        //RimitVelocityY();

    }

    /// <summary>
    /// 画面上部に到達した際、それ以上上にいかないようにする処理です
    /// </summary>
    protected void RimitScreenTop()
    {
        var ScreenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;
        if (transform.position.y > ScreenTop)
        {
            transform.position = new Vector2(transform.position.x, ScreenTop);
            if (rigidBody.velocity.y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            }
        }

    }
    /// <summary>
    /// 上方向への速度を制限する処理
    /// </summary>
    protected void RimitVelocityY()
    {
        if (rigidBody.velocity.y > maxVelocityY)
        {

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxVelocityY);
        }
    }

    /// <summary>
    /// 雨に打たれた際の処理
    /// </summary>
    protected void Do_Rainy()
    {
        if (IsRain)
        {
            PlayEffect(feverEffect);
        }
        else
        {
            StopEffect(feverEffect);
        }
    }


    /// <summary>
    /// エフェクトの再生処理
    /// </summary>
    /// <param name="effect"></param>
    public void PlayEffect(ParticleSystem effect)
    {
        if (effect.isPlaying)
        {
            return;
        }
        effect.Play();
    }

    /// <summary>
    /// エフェクトの停止処理
    /// </summary>
    /// <param name="effect"></param>
    public void StopEffect(ParticleSystem effect)
    {
        if (effect.isStopped)
        {
            return;
        }
        effect.Stop();
    }
    #region ステートの変更
    /// <summary>
    /// ステートを別のステートに変更
    /// </summary>
    /// <param name="newState"></param>
    protected virtual void ChangeState(CharacterState newState)
    {
        // 変更するステートが無いなら
        if (newState == null)
        {
            return;
        }
        // 現在のステートの終了時処理を行う
        if (state != null)
        {
            state.Exit();
        }
        // ステートを新しいステートに変更する
        state = newState;
        // 新しいステートの開始処理を行う
        state.Entry();
    }
    // 各ステートに変更するアクセサーメソッド
    /// <summary>
    /// 待機状態に変更
    /// </summary>
    public abstract void IdleStart();

    /// <summary>
    /// 走り状態に変更
    /// </summary>
    public abstract void RunStart();

    /// <summary>
    /// 空中状態に変更
    /// </summary>
    public abstract void AerialStart();

    /// <summary>
    /// 滑空状態に変更
    /// </summary>
    public abstract void GlideStart();

    /// <summary>
    /// 手すりの状態に変更
    /// </summary>
    public abstract void SlideStart();

    /// <summary>
    /// 手すりの後のジャンプ猶予状態に変更
    /// </summary>
    public abstract void AfterSlideStart();

    /// <summary>
    /// ブーストの状態に変更
    /// </summary>
    public abstract void BoostStart();
    
    /// <summary>
    /// ダウン状態に変更
    /// </summary>
    public abstract void Down();

    /// <summary>
    /// ゴール後の状態に変更
    /// </summary>
    public abstract void AfterGoalStart();
    #endregion
    #region 現在のステートを確認するためのget
    public abstract bool IsIdle       { get; }
    public abstract bool IsRun        { get; }
    public abstract bool IsAerial     { get; }
    public abstract bool IsGlide      { get; }
    public abstract bool IsSlide      { get; }
    public abstract bool IsAfterSlide { get; }
    public abstract bool IsBoost      { get; }
    public abstract bool IsDown       { get; }
    public abstract bool IsAfterGoal  { get; }
    #endregion
    #region 特定のアクションを行うか
    public abstract bool IsJumpStart  { get; }
    public abstract bool IsGlideStart { get; }
    public abstract bool IsGlideEnd   { get; }
    public abstract bool IsSlideStart { get; }
    public abstract bool IsSlideEnd   { get; }
    public abstract bool IsAttack     { get; }
    public abstract bool IsCharge     { get; }
    public abstract bool IsBoostStart { get; }
    #endregion
}