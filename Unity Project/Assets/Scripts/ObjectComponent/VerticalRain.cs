using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRain : MonoBehaviour
{
    /// <summary>
    /// switc用enum
    /// </summary>
    public enum RainMode
    {
        IDLE,        // 待機状態
        INIT,        // 準備状態
        INCREASE,    // 雨の勢いを増す
        DECREASE,    // 雨の勢いを弱める
    }

    [SerializeField]
    public RainMode mode = RainMode.IDLE;
    // コルーチン
    IEnumerator delayChangestate = null;
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
    // 雨の降る時間
    [SerializeField]
    float RainTime = 5f;
    // 最大増加量
    [SerializeField]
    float maxRate = 50f;
    [SerializeField]
    float maxSpeed = 30f;      

    private void Start()
    {
        // 変数初期化
        rainEffect = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        main = rainEffect.main;
        emission = rainEffect.emission;
    }

    private void Update()
    {
        SwichMode();
    }

    /// <summary>
    /// モード切替処理
    /// </summary>
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
                    // モード移行
                    ChangeMode(RainMode.INCREASE);
                    break;
                }
            case RainMode.INCREASE:
                {                    
                    // 雨を強くする
                    IncreaseRain();
                                     
                    if (delayChangestate == null)
                    {　 
                        // プレイヤーの雨フラグON
                        StartCoroutine(ChangePlayerIsRain(true,0.5f));
                        // 時間をおいてモード移行
                        delayChangestate = ChangeModeDelay(RainTime, RainMode.DECREASE);
                        StartCoroutine(delayChangestate);
                    }
                    break;
                }           
            case RainMode.DECREASE:
                {
                    // 雨が徐々にやんでいく処理
                    var completeDecrease = DecreaseRain();
                    // 雨のエフェクト量が0になったらエフェクト停止
                    if(completeDecrease)
                    {
                        audioSource.Stop();
                        // エフェクト停止
                        rainEffect.Stop();   
                        // プレイヤーの雨フラグOFF
                        StartCoroutine(ChangePlayerIsRain(false ,1f));
                        // モード移行
                        ChangeMode(RainMode.IDLE);
                    }
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

    /// <summary>
    /// 遅延後モード移行処理
    /// </summary>
    /// <param name="flug"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ChangePlayerIsRain(bool flug, float delay = 0)
    {
        // 時間待機
        yield return new WaitForSeconds(delay);
        // プレイヤーの雨フラグON
        foreach (Player player in SceneController.Instance.playerEntityData.players.Values)
        {
            player.IsRain = flug;
        }
    }

    /// <summary>
    /// 遅延後モード移行処理
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    IEnumerator ChangeModeDelay(float delay, RainMode mode)
    {
        // 時間待機
        yield return new WaitForSeconds(delay);        
        // モード移行
        ChangeMode(mode);
        // コルーチン解放
        delayChangestate = null;
        yield break;
    }

    /// <summary>
    /// 他のクラスから雨を降らせる際使用
    /// </summary>
    public void StartRain()
    {
        mode = RainMode.INIT;
    }

    /// <summary>
    /// 雨を弱める処理
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
    /// 雨を激しくする処理
    /// </summary>
    bool DecreaseRain()
    {
        // エフェクトの量増加処理
        var completeDecreaseRate = DecreaseRainRate();
        // エフェクトスピード増加処理        
        var completeDecreaseSpeed = DecreaseRainSpeed();
        DownVolume();
        // 処理が完了したらtrueを返す
        if(completeDecreaseRate && completeDecreaseSpeed)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// エフェクトの量増加処理
    /// </summary>
    void IncreaseRainRate()
    {
        
        // 最大値制限
        if (emission.rateOverTime.constantMax >= maxRate) { return; }

        // エフェクト量増加処理
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constantMin + addRate * Time.deltaTime,
                                                                 emission.rateOverTime.constantMax + addRate * Time.deltaTime);
    }


    /// <summary>
    /// 雨のエフェクト量減少処理
    /// </summary>
    bool DecreaseRainRate()
    {
        
        var CurveMin = emission.rateOverTime.constantMin;
        var CurveMax = emission.rateOverTime.constantMax;

        // 最小値制限
        if (CurveMin <= 0 || CurveMax <= 0)
        {           
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(0,0);
            // 最小になったらtrueを返す
            return true;
        }                    
        // エフェクト量
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constantMin - addRate * Time.deltaTime,
                                                                 emission.rateOverTime.constantMax - addRate * Time.deltaTime);
        return false;
    }

    /// <summary>
    /// 雨のエフェクトの速度増加処理
    /// </summary>
    void IncreaseRainSpeed()
    {
        // 最大値制限
        if (main.startSpeed.constantMax >= maxSpeed) { return; }

        // エフェクト量増加処理
        main.startSpeed = new ParticleSystem.MinMaxCurve(main.startSpeed.constantMin + addSpeed * Time.deltaTime,
                                                                 main.startSpeed.constantMax + addSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 雨のエフェクトの速度減少処理
    /// </summary>
    bool DecreaseRainSpeed()
    {
        var CurveMin = main.startSpeed.constantMin;
        var CurveMax = main.startSpeed.constantMax;

        // 最小値制限
        if (CurveMin < 0 || CurveMax < 0)
        {
            main.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);
            // 最小になったらtrueを返す
            return true;
        }
        // エフェクト量
        main.startSpeed = new ParticleSystem.MinMaxCurve(main.startSpeed.constantMin - addSpeed * Time.deltaTime,
                                                                 main.startSpeed.constantMax - addSpeed * Time.deltaTime);
        return false;
    }



}
