using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFade : MonoBehaviour
{
    // スプライトレンダラー
    SpriteRenderer sprite;
    // α値の最大
    [SerializeField]
    float maxAlpha = 70;
    // α値の増加量
    [SerializeField]
    float addAlpha = 1f;
    // α値の減少量
    [SerializeField]
    float minusAlpha = 0.5f;
    // 白味の増加量
    [SerializeField]
    float addWhite = 10;
    // 雨のオブジェクト
    VerticalRain rain;
    // 雨の状態
    VerticalRain.RainMode rainMode;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネント取得
        sprite = GetComponent<SpriteRenderer>();
        rain = transform.root.transform.Find("Rain").GetComponent<VerticalRain>();        
    }

    // Update is called once per frame
    void Update()
    {
        // 雨のモード
        rainMode = rain.mode;

        // 雨の状態によって色を変える
        switch(rainMode)
        {         
            // 雨が強くなっているとき
            case VerticalRain.RainMode.INCREASE:
                {
                    // フェードの色はデフォルトで黒いので
                    // α値を上げる
                    IncreaseAlpha();
                    break;
                }
            case VerticalRain.RainMode.DECREASE:
                {
                    // 天気をよくする
                    ChangeFineWeather();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    /// <summary>
    /// α値を増やしていく処理
    /// </summary>
    void IncreaseAlpha()
    {
        // 現在のα値を256段階に変更
        var spriteAlpha = sprite.color.a * 255;       
        // 変更後のα値
        var nextAlpha = spriteAlpha + addAlpha * Time.deltaTime;
        // 最大値の制限処理
        nextAlpha = Mathf.Clamp(nextAlpha, 0, maxAlpha);
        // α値セット
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, nextAlpha / 255);
    }

    /// <summary>
    /// 晴れに近づけていく処理
    /// </summary>
    void ChangeFineWeather()
    {
        // α値の減算処理
        DecreaseAlpha();
        // 色を白に近づけていく処理
        ChangeToWhite();
    }

    /// <summary>
    /// α値を減算する処理
    /// </summary>
    void DecreaseAlpha()
    {
        // 現在のα値を256段階に変更
        var spriteAlpha = sprite.color.a * 255;
        // 変更後のα値
        var nextAlpha = sprite.color.a * 255 - minusAlpha * Time.deltaTime;
        // 最小値の制限処理
        nextAlpha = Mathf.Clamp(nextAlpha, 0, 255);
        // α値セット
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, nextAlpha / 255);
    }

    /// <summary>
    /// 白味を増していく処理
    /// </summary>
    void ChangeToWhite()
    {
        // 増加させる分の色
        var addColor = new Color(addWhite, addWhite, addWhite, 0) / 255;
        // 色の加算　
        sprite.color += addColor * Time.deltaTime;
        // 色の変更制限
        var nextRed = Mathf.Clamp01(sprite.color.r);
        var nextGreen = Mathf.Clamp01(sprite.color.g);
        var nextBlue = Mathf.Clamp01(sprite.color.b);
        // 制限した色のセット
        sprite.color = new Color(nextRed, nextGreen, nextBlue, sprite.color.a);
    }
}
