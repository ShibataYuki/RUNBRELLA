using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGauge : MonoBehaviour
{
    // 通常時のゲージの画像のリスト
    [SerializeField]
    private List<Sprite> gaugeSprites = new List<Sprite>();
    // チャージ中のゲージの画像のリスト
    [SerializeField]
    private List<Sprite> chargeGaugeSprites = new List<Sprite>();
    // 表示機能のリスト
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        // ゲージの親オブジェクトの参照を取得
        var gauge = transform.parent.Find("Gauge");
        for(int i = 1; i <= 5; i++)
        {
            // 1ゲージ分のオブジェクトの参照を取得
            var gaugeCover = gauge.Find(string.Format("Gauge_Cover{0}", i)).gameObject;
            // スプライトレンダラーの取得
            var coverSpriteRenderer = gaugeCover.GetComponent<SpriteRenderer>();
            //リストに追加
            spriteRenderers.Add(coverSpriteRenderer);
        }
    }

    /// <summary>
    /// 画像をセットしていくつチャージしているか示す
    /// </summary>
    /// <param name="chargeCount"></param>
    public void SetChargeSprite(int chargeCount)
    {
        // チャージ中の画像をセット
        for(int i = 0; i < chargeCount; i++)
        {
            spriteRenderers[i].sprite = chargeGaugeSprites[i];
        }
        // 通常時の画像をセット
        for(int i = chargeCount; i < 5; i++)
        {
            spriteRenderers[i].sprite = gaugeSprites[i];
        }
    }
}
