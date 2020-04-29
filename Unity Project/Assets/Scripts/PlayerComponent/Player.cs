using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーのナンバー
    public PLAYER_NO playerNO { get; set; } = 0;
    // プレイヤーのコントローラナンバー
    public CONTROLLER_NO controllerNo { get; set; } = 0;
    // キャラクターのタイプ
    public string Type { get; set; } = "A";
    // 地面にいるか    
    [SerializeField]
    bool isGround = false;
    public bool IsGround { set { isGround = value; } get { return isGround; } }
    // プレイヤーステート   
    public IState state = null;
    // プレイヤーがダウンしている時間
    public float downTime = 0;
    // プレイヤーの基本の加速度
    [SerializeField]
    private float baseAddSpeed = 1.5f;
    public float BaseAddSpeed { get { return baseAddSpeed; } set { baseAddSpeed = value; } }
    // 最低スピード
    [SerializeField]
    private float baseSpeed = 6;
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }
    [SerializeField]
    private float maxSpeed = 10;
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    // 雨に当たっているか
    [SerializeField]
    public bool IsRain  = false;
    [SerializeField]
    float maxVelocityY = 15f;
    // プレイヤーの速度保存領域
    public float VelocityXStorage { get; set; } = 0;
    // リジッドボディ
    private Rigidbody2D rigidBody;
    public Rigidbody2D Rigidbody{ get { return rigidBody; } }
    private PlayerSlide playerSlide;
    // アニメーター
    private Animator animator;

    // AnimatorのパラメーターID
    readonly int jumpID   = Animator.StringToHash("IsGround");
    readonly int runID    = Animator.StringToHash("Velocity");
    readonly int sliderID = Animator.StringToHash("IsSlider");
    readonly int gliderID = Animator.StringToHash("IsGlide");
    readonly int downID   = Animator.StringToHash("IsDown");
    readonly int boostID  = Animator.StringToHash("IsBoost");

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
    private string stateName;
#endif

    private void Awake()
    {
        feverEffect = transform.Find("FeverEffect").GetComponent<ParticleSystem>();
        boostEffect = transform.Find("BoostEffect").GetComponent<ParticleSystem>();
        chargeingEffect = transform.Find("ChargeEffects/Charging").GetComponent<ParticleSystem>();
        chargeSignal = transform.Find("ChargeEffects/ChargeSignal").GetComponent<ParticleSystem>();
        chargePauseEffect = transform.Find("ChargeEffects/ChargePause").GetComponent<ParticleSystem>();
        chargeMaxEffect = transform.Find("ChargeEffects/ChargeMax").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();        
        // テキストを読み込むファイル名
        string fileName = nameof(Player) + "Data" + Type;
        // テキストの読み込み
        downTime = TextManager.Instance.GetValue_float(fileName, nameof(downTime));
        baseSpeed = TextManager.Instance.GetValue_float(fileName, nameof(baseSpeed));
        maxVelocityY = TextManager.Instance.GetValue_float(fileName, nameof(maxVelocityY));
    }

// Update is called once per frame
void Update()
    {           
        // stateのDo関数を呼ぶ
        state.Do(controllerNo);
        Do_AnyState();
#if UNITY_EDITOR
        // 現在のステートをInspecter上に表示
        stateName = state.ToString();
        if((int)controllerNo==1)
        {
            //Debug.Log(VelocityXStorage);
        }
#endif
    }

    /// <summary>
    /// アニメーターにパラメータをセット
    /// </summary>
    private void SetAnimator()
    {
        animator.SetBool(jumpID, isGround);
        animator.SetBool(sliderID, (playerSlide.RayHit || playerSlide.IsColliderHit) && state.ToString() == "PlayerSlideState");
        animator.SetFloat(runID, Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool(gliderID, state.ToString() == "PlayerGlideState");
        animator.SetBool(downID, state.ToString() == "PlayerDownState");
        animator.SetBool(boostID, state.ToString() == "PlayerBoostState");
    }

    private void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        state.Do_Fix(controllerNo);
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
        RimitVelocityY();

    }

    /// <summary>
    /// 画面上部に到達した際、それ以上上にいかないようにする処理です
    /// </summary>
    private void RimitScreenTop()
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
    private void RimitVelocityY()
    {
        if (rigidBody.velocity.y > maxVelocityY)
        {

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxVelocityY);
        }
    }

    /// <summary>
    /// 雨に打たれた際の処理
    /// </summary>
    void Do_Rainy()
    {
        if(IsRain)
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
        if(effect.isPlaying)
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
   
}
