using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーID
    public int ID { get; set; } = 0;
    // 地面にいるか    
    [SerializeField]
    bool isGround = false;
    public bool IsGround { set { isGround = value; } get { return isGround; } }
    // たまに当たったか
    [SerializeField]
    bool isHitBullet = false;
    public bool IsHitBullet { set { isHitBullet = value; } get { return isHitBullet; } }
    // プレイヤーステート   
    public IState state = null;
    // プレイヤーがダウンしている時間
    public float downTime = 0;

    [SerializeField]
    private float baseSpeed = 6;
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }

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
    
    public ParticleSystem feverEffect;
    public ParticleSystem boostEffect;

#if UNITY_EDITOR
    // ステートの名前をデバッグ表示する変数
    [SerializeField]
    private string stateName;
#endif
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();
        feverEffect = transform.Find("FeverEffect").GetComponent<ParticleSystem>();
        boostEffect = transform.Find("BoostEffect").GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {           
        // stateのDo関数を呼ぶ
        state.Do(ID);
        Do_AnyState();
#if UNITY_EDITOR
        // 現在のステートをInspecter上に表示
        stateName = state.ToString();
        if(ID==1)
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
        state.Do_Fix(ID);
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

    /// <summary>
    /// 最高速度保存処理
    /// </summary>
    public void SaveVelocity()
    {
        if(rigidBody.velocity.x > BaseSpeed / 2)
        {
            VelocityXStorage = rigidBody.velocity.x;
        }
    }

    /// <summary>
    /// 最高速度リセット
    /// </summary>
    public void ResetVelocityXStorage()
    {
        VelocityXStorage = 0;
    }




   
}
