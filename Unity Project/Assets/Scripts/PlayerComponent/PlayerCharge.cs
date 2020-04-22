using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharge : MonoBehaviour
{
    // チャージ時間
    float chargeTime = 0.0f;
    // チャージ数
    int chargeCount = 0;
    // 1回チャージするまでの時間
    float oneChargeTime = 1.0f;

    // 必要なコンポーネント
    Player player;
    PlayerAttack playerAttack;
    PlayerBoost playerBoost;
    // 子オブジェクトのコンポーネント
    ChargeGauge chargeGauge;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        player = GetComponent<Player>();
        playerAttack = GetComponent < PlayerAttack>();
        playerBoost = GetComponent<PlayerBoost>();
        // チャージ状態を表すゲージオブジェクト
        var chargeGauge = transform.Find("ChargeGauge").gameObject;
        // ゲージのオブジェクトからのアニメーターコンポーネントの取得
        this.chargeGauge = chargeGauge.GetComponent<ChargeGauge>();
    }

    private void Update()
    {
        chargeGauge.SetChargeSprite(chargeCount);
    }

    /// <summary>
    /// チャージ処理
    /// </summary>
    public void Charge()
    {
        // プレイヤーのチャージが出来るなら
        if(chargeCount < playerAttack.NowBulletCount)
        {
            //前回フレームのチャージ数を保存
            var beforeChargeCount = chargeCount;
            // 経過時間を計測
            chargeTime += Time.deltaTime;
            // チャージ数を計算
            chargeCount = (int)(chargeTime / oneChargeTime) + 1;
            // エフェクトをONにする
            player.PlayEffect(player.chargeingEffect);
            // 今回のフレームでチャージされたなら
            if (chargeCount > beforeChargeCount)
            {
                // エフェクトをONにする。
                player.PlayEffect(player.chargeSignal);
            }
        }
        else
        {
            // エフェクトをOFFにする。
            player.StopEffect(player.chargeingEffect);
        }
        // チャージが出来ない、又はチャージが完了したら
        if (chargeCount >= playerAttack.NowBulletCount)
        {
            // チャージ数をゲージのエネルギー数に合わせる
            chargeCount = playerAttack.NowBulletCount;
            // チャージ時間を計算
            chargeTime = (chargeCount - 1) * oneChargeTime;
        }
    }

    /// <summary>
    /// ブースト出来るかチェックして、チャージ数をブーストに反映させる
    /// </summary>
    /// <returns>ブースト出来るかできないか</returns>
    public bool BoostCheck()
    {
        // チャージされていて、かつエネルギー以下なら
        if (chargeCount > 0 && chargeCount <= playerAttack.NowBulletCount)
        {
            // チャージ数をブーストに伝える
            playerBoost.GaugeCount = chargeCount;
            // チャージをリセット
            ChargeReset();
            return true;
        }
        else
        {
            // チャージをリセット
            ChargeReset();
            return false;
        }
    }

    /// <summary>
    /// 現在のチャージをリセット
    /// </summary>
    public void ChargeReset()
    {
        chargeCount = 0;
        chargeTime = 0.0f;
        player.StopEffect(player.chargeingEffect);
        player.StopEffect(player.chargeSignal);
    }
}
