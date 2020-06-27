using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// switc用enum
/// </summary>

public class Rain : MonoBehaviour
{
    public enum RainMode
    {
        IDLE,        // 待機状態
        INIT,        // 準備状態
        KEEP_RAIN,   // 雨の勢いを保持
        INCREASE,    // 雨の勢いを増す
        DECREASE,    // 雨の勢いを弱める
        RESET,       // 終了後処理 
    }
    [SerializeField]
    public RainMode mode = RainMode.IDLE;   
    // パーティクルシステムと各モジュール
    ParticleSystem rainEffect;
    ParticleSystem.MainModule main;
    ParticleSystem.EmissionModule emission;  
    private AudioSource audioSource;
    private float volume = 0.0f;

    // 豪雨時のエフェクト数増加量
    [SerializeField]
    float addRate = 0;
    // 豪雨時のスピード増加量
    [SerializeField]
    float addSpeed = 0f;
    // 一番強い状態にしておく時間
    [SerializeField]
    float rainTime = 0;
    // エフェクトの最大量
    [SerializeField]
    float maxRate = 0;
    // エフェクトの最大速度
    [SerializeField]
    float maxSpeed = 0;
    // 雨の強さが最高に至るまでの時間
    [SerializeField]
    public float increaseTime = 0;
    // 雨の強さが最低に至るまでの時間
    [SerializeField]
    public float decreaseTime = 0;
    public float rainPercentage  = 0;
    RainIconFactory rainIconFactory = null;
    ParticleSystem.MinMaxCurve baseRate;
    ParticleSystem.MinMaxCurve baseSpeed;
    
    private void Start()
    {        
        // テキスト読み込み
        SheetToDictionary.Instance.TextToDictionary("Rain",out var textDataDic);
        // データ代入
        addRate = textDataDic["雨の量が増えるスピード"];
        addSpeed = textDataDic["雨の速度が増えるスピード"];
        maxRate = textDataDic["雨の最大の量"];
        maxSpeed = textDataDic["雨の最大の速度"];
        increaseTime = textDataDic["雨の強さが最大になるのにかかる時間(秒)"];
        rainTime = textDataDic["雨が最大の強さをキープする時間(秒)"];
        decreaseTime = textDataDic["雨が止むまでにかかる時間(秒)"];
        // 変数初期化
        rainEffect = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        main = rainEffect.main;
        emission = rainEffect.emission;
        rainIconFactory = GameObject.Find("RainIconFactory").GetComponent<RainIconFactory>();
        baseRate = emission.rateOverTime;
        baseSpeed = main.startSpeed;
    }

    private void Update()
    {     
        SwichMode();
    }

    /// <summary>
    /// モード切替処理
    /// </summary>
    float keepModeRainTimer = 0;    //　キープモード用経過時間管理変数
    void SwichMode()
    {
        switch(mode)
        {
            // 待機状態
            case RainMode.IDLE:
                {
                    break;
                }
            case RainMode.INIT:
                {
                    // エフェクトの再生
                    rainEffect.Play();
                    volume = 0.0f;
                    SetVolume();
                    audioSource.Play();
                    emission.rateOverTime = baseRate;
                    main.startSpeed = baseSpeed;
                    // モード移行
                    ChangeMode(RainMode.INCREASE);   
                    
                    break;
                }
            case RainMode.INCREASE:
                {                  
                    // 雨を強くする
                    IncreaseRain();
                    // 雨が最大勢力の何パーセントほどの強さか
                    rainPercentage = RainPercentage();
                    
                    // 5%　以上なら
                    if (rainPercentage >= 5)
                    {
                        // プレイヤーの雨フラグON
                        ChangePlayerIsRain(true);                                               
                    }
                    if (rainPercentage >= 100)
                    {
                        ChangeMode(RainMode.KEEP_RAIN);                       
                    }
                    break;
                }
            case RainMode.KEEP_RAIN:
                {
                    
                    keepModeRainTimer += Time.deltaTime;
                    if(keepModeRainTimer >= rainTime)
                    {
                        ChangeMode(RainMode.DECREASE);
                        keepModeRainTimer = 0;                        
                    }
                    
                    break;
                }
            case RainMode.DECREASE:
                {
                    // 雨が徐々にやんでいく処理
                    DecreaseRain();
                    // 雨が最大勢力の何パーセントほどの強さか
                    rainPercentage = RainPercentage();
                 
                    // 5%　以下なら
                    if (rainPercentage <= 5)
                    {
                        // プレイヤーの雨フラグOFF
                        ChangePlayerIsRain(false);
                        rainIconFactory.KeepObjSetFalse();
                        audioSource.Stop();
                        // エフェクト停止
                        rainEffect.Stop();
                    }
                    if (rainPercentage <= 0)
                    {
                        // モード移行
                        ChangeMode(RainMode.RESET);                       
                    }
                                           
                    break;
                }
            case RainMode.RESET:
                {
                    ChangeMode(RainMode.IDLE);
                   
                    break;
                }
            default:
                {
                    Debug.Assert(false, "不正なモードです");
                    break;
                }

        }

        
    }
   
    /// <summary>
    /// モード移行処理
    /// </summary>
    /// <param name="mode"></param>
    void ChangeMode(RainMode mode)
    {
        this.mode = mode;
    }
    
    void ChangePlayerIsRain(bool flag)
    {
        // プレイヤーの雨フラグON
        foreach (Character character in SceneController.Instance.players.Values)
        {
            if(character.IsRain == flag) { return; }
            character.IsRain = flag;
        }
    }

    IEnumerator delayMethod(Action action)
    {
        yield return new WaitForSeconds(1f);
        action();
    }
   

    /// <summary>
    /// 他のクラスから雨を降らせる際使用
    /// </summary>
    public void StartRain()
    {
        // 待機状態ならINITへ
        // すでに起動中なら増加処理へ
        if(mode == RainMode.IDLE)
        {
            mode = RainMode.INIT;
        }
        else
        {
            mode = RainMode.INCREASE;
        }
    }

    

    float RainPercentage()
    {        
        var nowRainRate = emission.rateOverTime.constantMax;
        var rainPercentage = (nowRainRate - baseRate.constantMax) / (maxRate - baseRate.constantMax) * 100;
        rainPercentage = Mathf.Clamp(rainPercentage, 0, 100);
        return rainPercentage;
    }


    /// <summary>
    /// 雨を強くする処理
    /// </summary>
    void IncreaseRain()
    {
        // エフェクトの量増加処理
        IncreaseRainRate();
        // エフェクトスピード増加処理        
        IncreaseRainSpeed();
        AddVolume();
    }

    /// <summary>
    /// ボリュームを上げる
    /// </summary>
    void AddVolume()
    {
        volume += (maxRate / addRate) * Time.deltaTime;
        volume = Mathf.Clamp01(volume);
        SetVolume();
    }

    /// <summary>
    /// ボリュームを下げる
    /// </summary>
    void DownVolume()
    {
        volume -= (maxRate / addRate) * Time.deltaTime;
        volume = Mathf.Clamp01(volume);
        SetVolume();
    }

    /// <summary>
    /// ボリュームをセット
    /// </summary>
    void SetVolume()
    {
        audioSource.volume = volume;
    }
   
    /// <summary>
    /// 雨を弱くする処理
    /// </summary>
    void DecreaseRain()
    {
        // エフェクトの量減少処理
        DecreaseRainRate();
        // エフェクトスピード減少処理        
        DecreaseRainSpeed();
        DownVolume();       
    }

    /// <summary>
    /// エフェクトの量増加処理
    /// </summary>
    void IncreaseRainRate()
    {        
        // 最大値制限
        if (emission.rateOverTime.constantMax >= maxRate) { return; }

        // 最大値/指定秒数　= 1秒あたりの増加量
        float increasePerSecond = (maxRate - baseRate.constantMax) / increaseTime;
        // 増加後の値を計算
        var curveMin = emission.rateOverTime.constantMin + increasePerSecond * Time.deltaTime;
        var curveMax = emission.rateOverTime.constantMax + increasePerSecond * Time.deltaTime;
        // 最大値制限
        curveMax = Mathf.Clamp(curveMax, 0, maxRate);
        // エフェクト量増加処理
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(curveMin, curveMax);

    }



    void DecreaseRainRate()
    {
        // 最小値制限
        if (emission.rateOverTime.constantMax <= 0) { return; }

        // 最大値/指定秒数　= 1秒あたりの増加量
        float decreasePerSecond = (maxRate - baseRate.constantMax) / decreaseTime;
        // 増加後の値を計算
        var curveMin = emission.rateOverTime.constantMin - decreasePerSecond * Time.deltaTime;
        var curveMax = emission.rateOverTime.constantMax - decreasePerSecond * Time.deltaTime;
        // 最大値制限
        curveMax = Mathf.Clamp(curveMax, 0, maxRate);
        curveMin = Mathf.Clamp(curveMin, 0, maxRate);
        // エフェクト量増加処理
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(curveMin, curveMax);
    }

    /// <summary>
    /// 雨のエフェクトの速度増加処理
    /// </summary>
    void IncreaseRainSpeed()
    {       
        // 最大値制限
        if (main.startSpeed.constantMax >= maxSpeed) { return; }

        // 最大値/指定秒数　= 1秒あたりの増加量
        float increasePerSecond = (maxSpeed - baseSpeed.constantMax) / increaseTime;
        // 増加後の値を計算
        var curveMin = main.startSpeed.constantMin + increasePerSecond * Time.deltaTime;
        var curveMax = main.startSpeed.constantMax + increasePerSecond * Time.deltaTime;
        // 最大値制限
        curveMax = Mathf.Clamp(curveMax, 0, maxSpeed);
        // エフェクト速度増加処理
        main.startSpeed = new ParticleSystem.MinMaxCurve(curveMin, curveMax);

    }
    

    void DecreaseRainSpeed()
    {
       
        // 最小値制限
        if (main.startSpeed.constantMax <= 0) { return; }

        // 最大値/指定秒数　= 1秒あたりの増加量
        float decreasePerSecond = (maxSpeed - baseSpeed.constantMax) / decreaseTime;
        // 増加後の値を計算
        var curveMin = main.startSpeed.constantMin - decreasePerSecond * Time.deltaTime;
        var curveMax = main.startSpeed.constantMax - decreasePerSecond * Time.deltaTime;
        // 最大値制限
        curveMax = Mathf.Clamp(curveMax, 0, maxSpeed);
        curveMin = Mathf.Clamp(curveMin, 0, maxSpeed);
        // エフェクト量増加処理
        main.startSpeed = new ParticleSystem.MinMaxCurve(curveMin, curveMax);

    }

}
