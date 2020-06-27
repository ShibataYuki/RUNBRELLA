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
    // 雨のオブジェクト
    Rain rain;
    // 雨の状態
    Rain.RainMode rainMode;
    float increaseTime;
    float decreaseTime;

    // Start is called before the first frame update
    void Start()
    {
        // テキスト読み込み
        SheetToDictionary.Instance.TextToDictionary("Rain", out var textDataDic);
        // データ代入
        maxAlpha = textDataDic["雨が降った際の空の黒さの最大値(0～255)"];
        // コンポーネント取得
        sprite = GetComponent<SpriteRenderer>();
        rain = transform.root.transform.Find("Rain").GetComponent<Rain>();
        increaseTime = rain.increaseTime;
        decreaseTime = rain.decreaseTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // α値の変更
        AlphaByRainPercentage();
        // 色の変更
        ColorByRainPercentage();

    }

    /// <summary>
    /// 雨の勢いのパーセンテージによってα値を変化させます
    /// </summary>
    void AlphaByRainPercentage()
    {
        var maxAlpha01 = maxAlpha / 255f;
        // 雨の強さ1パーセント当たりのα値
        var alphaParPercentage = maxAlpha01 / 100f;
        var newColor = sprite.color;
        // 1パーセント当たりのα値 * 雨の強さ
        newColor.a = alphaParPercentage * rain.rainPercentage;
        // 最大最小値制限
        newColor.a = Mathf.Clamp(newColor.a, 0, maxAlpha01);
        // 変更後色セット
        sprite.color = newColor;
    }


    void ColorByRainPercentage()
    {
        var maxColor01 = 1f;
        //  maxColor01の1パーセント
        var onePercentByMaxColor = maxColor01 / 100f;
        // 1パーセント当たりの色の彩度 * 雨の強さ
        // 雨のつよさが100％の時Color(0,0,0)にしたいから1から引いて反転させている
        var colorParPercentage = 1 - (onePercentByMaxColor * rain.rainPercentage);
        // 最大最小値制限
        colorParPercentage = Mathf.Clamp01(colorParPercentage);
        // 変更後色セット
        sprite.color = new Color(colorParPercentage, colorParPercentage, colorParPercentage, sprite.color.a);
        
    }

}
