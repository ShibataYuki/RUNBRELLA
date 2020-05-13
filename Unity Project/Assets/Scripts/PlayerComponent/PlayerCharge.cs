﻿using System.Collections;
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
        // パラメータをセット
        ReadTextParameter();
    }

    /// <summary>
    /// テキストからパラメータを読み込む
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "";
        switch (player.charAttackType)
        {
            case GameManager.CHARATTACKTYPE.GUN:
                textName = "Chara_Gun";
                break;
            case GameManager.CHARATTACKTYPE.SWORD:
                textName = "Chara_Sword";
                break;
        }
        try
        {
            // テキストの中のデータをセットするディクショナリー
            SheetToDictionary.Instance.TextToDictionary(textName, out var chargeDictionary);

            try
            {
                // ファイル読み込み
                chargeTime = chargeDictionary["1ゲージチャージする秒数"];
            }
            catch
            {
                Debug.Assert(false, nameof(PlayerCharge) + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                + textName + "の読み込みに失敗しました。");
        }
    }


    private void Update()
    {
        chargeGauge.SetChargeSprite(chargeCount);
    }

    /// <summary>
    /// チャージ処理
    /// </summary>
    private void Charge()
    {
        // チャージが出来ないステートなら
        if((player.state != PlayerStateManager.Instance.playerAerialState)
            && (player.state != PlayerStateManager.Instance.playerRunState)
            && (player.state != PlayerStateManager.Instance.playerGlideState)
            && (player.state != PlayerStateManager.Instance.playerAfterSlideState))
        {
            return;
        }
        // プレイヤーのチャージが出来るなら
        if(chargeCount < playerAttack.NowBulletCount)
        {
            //前回フレームのチャージ数を保存
            var beforeChargeCount = chargeCount;
            // 経過時間を計測
            chargeTime += Time.deltaTime;
            // チャージ数を計算
            chargeCount = (int)(chargeTime / oneChargeTime);
            // エフェクトをONにする
            player.PlayEffect(player.chargeingEffect);
            // チャージが停止中のエフェクトをOFFにする
            ChargePauseEffectStop();
            // 今回のフレームでチャージされたなら
            if (chargeCount > beforeChargeCount)
            {
                // エフェクトをONにする。
                player.PlayEffect(player.chargeSignal);
            }
        }
        else
        {
            // エフェクトを一時停止する。
            ChargeStop();
        }
        // チャージが出来ない、又はチャージが完了したら
        if (chargeCount >= playerAttack.NowBulletCount)
        {
            // チャージ数をゲージのエネルギー数に合わせる
            chargeCount = playerAttack.NowBulletCount;
            // チャージ時間を計算
            chargeTime = chargeCount * oneChargeTime;
        }
    }

    /// <summary>
    /// チャージが一時停止中のエフェクトを停止する
    /// </summary>
    private void ChargePauseEffectStop()
    {
        if (player.chargePauseEffect.isPlaying == true)
        {
            // エフェクトを停止する
            player.StopEffect(player.chargePauseEffect);
            // すでに出ているエフェクトを非表示にする
            player.chargePauseEffect.Clear();
        }
        if(player.chargeMaxEffect.isPlaying == true)
        {
            // エフェクトを停止する
            player.StopEffect(player.chargeMaxEffect);
            // すでに出ているエフェクトを非表示にする
            player.chargeMaxEffect.Clear();
        }
    }

    /// <summary>
    /// チャージエフェクトの停止
    /// </summary>
    public void ChargeStop()
    {
        // エフェクトを一時停止する。
        player.StopEffect(player.chargeingEffect);
        player.chargeingEffect.Clear();
        if(chargeCount > 0 && chargeCount < 5)
        {
            // チャージが一時停止中の場合用のエフェクトを再生する
            player.PlayEffect(player.chargePauseEffect);
        }
        else if(chargeCount >= 5)
        {
            // チャージがMAXの場合のエフェクトを再生する
            player.PlayEffect(player.chargeMaxEffect);
        }
        // チャージ時間を計算
        chargeTime = chargeCount * oneChargeTime;
    }

    /// <summary>
    /// ブースト出来るかチェックして、チャージ数をブーストに反映させる
    /// </summary>
    /// <returns>ブースト出来るかできないか</returns>
    private bool BoostCheck()
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
        // エフェクトが一時停止されている可能性があるので一度再生してから停止する
        ChargePauseEffectStop();
        player.StopEffect(player.chargeingEffect);
        player.StopEffect(player.chargeSignal);
    }

    /// <summary>
    /// ブーストのキーの入力を確認する
    /// </summary>
    /// <param name="controllerNo"></param>
    public void BoostKeyCheck(CONTROLLER_NO controllerNo)
    {
        // キーを長押ししたなら
        if (InputManager.Instance.BoostKeyHold(controllerNo))
        {
            // チャージする
            SceneController.Instance.playerEntityData.playerCharges[controllerNo].Charge();
        }
        // キーを離したなら
        else if (InputManager.Instance.BoostKeyOut(controllerNo))
        {
            // ブーストが出来ないステートなら
            if ((player.state != PlayerStateManager.Instance.playerAerialState)
                && (player.state != PlayerStateManager.Instance.playerRunState)
                && (player.state != PlayerStateManager.Instance.playerGlideState)
                && (player.state != PlayerStateManager.Instance.playerAfterSlideState))
            {
                // チャージをリセットする
                ChargeReset();
                return;
            }
            else
            {
                // ブースト出来るなら
                if (SceneController.Instance.playerEntityData.playerCharges[controllerNo].BoostCheck())
                {
                    // ブーストを開始する
                    PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerBoostState, controllerNo);
                }
            }
        }
    }
}
