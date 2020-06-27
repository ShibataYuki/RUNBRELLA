using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackRain : MonoBehaviour
{
    // パーティクルシステムと各モジュール
    ParticleSystem rain;
    ParticleSystem.MainModule main;
    ParticleSystem.EmissionModule emission;

    // 通常時パラメータ
    ParticleSystem.MinMaxCurve baseRate;
    ParticleSystem.MinMaxCurve baseSpeed;
    float baseSize;
    
    // 豪雨時のエフェクト数増加量
    [SerializeField]
    float addRate = 20f;
    // 豪雨時のスピード増加量
    [SerializeField]
    float addSpeed = 5f;
    // 豪雨時のサイズ増加量
    [SerializeField]
    float addSize = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rain = GetComponent<ParticleSystem>();
        main = rain.main;
        emission = rain.emission;
        baseRate = emission.rateOverTime;
        baseSpeed = main.startSpeed;
        baseSize = main.startSize.constant;
        // ファイル名
        var fileName = nameof(BackRain) + "Data";
        // テキスト読み込み
        addRate = TextManager.Instance.GetValue_float(fileName, nameof(addRate));
        addSpeed = TextManager.Instance.GetValue_float(fileName, nameof(addSpeed));
        addSize = TextManager.Instance.GetValue_float(fileName, nameof(addSize));
    }
    
    /// <summary>
    /// 豪雨時の各パラメータ増加処理
    /// </summary>
    public void ChangeHeavyRain()
    {
        // エフェクト量
        emission.rateOverTime =  new ParticleSystem.MinMaxCurve(baseRate.constantMin + addRate,
                                                                 baseRate.constantMax + addRate);
        // スピード
        main.startSpeed = new ParticleSystem.MinMaxCurve(baseSpeed.constantMin + addSpeed,
                                                         baseSpeed.constantMax + addSpeed);
        // サイズ
        main.startSize = baseSize + addSize; 
    }

    /// <summary>
    /// 通常の雨の勢いに戻す処理
    /// </summary>
    public void ChangeaNomalRain()
    {
        // エフェクト量
        emission.rateOverTime = baseRate;
        // スピード
        main.startSpeed = baseSpeed;
        // サイズ
        main.startSize = baseSize;
    }
}
