using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;    
    Player player;
    PlayerRun playerRun;
    // 速度減衰値
    float decaySpeed;
    // 走っている状態の速度を基準としてその何パーセントの速さにするか
    [SerializeField]
    float EagingSpeedPercent = 0;
    // 毎フレーム前フレームの落下速度の何パーセントの速さにするか
    [SerializeField]
    float EasingVelocityYPercent = 0;

	
    // 前方に地面があるかチェックするコンポーネント
    private PlayerAerial playerAerial = null;
    // 傘を開いた時のSE
    [SerializeField]
    private AudioClip openSE = null;
    // SEのボリューム
    private float SEVolume = 0.5f;

    // 加える力    
    private float grideAddSpeed;
    [SerializeField]
    private float maxSpeed = 0;

    private void Start()
    {
        
        
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerRun = GetComponent<PlayerRun>();      
        playerAerial = GetComponent<PlayerAerial>();
        // テキストの読み込み       
        // データ代入
        ReadTextParameter();
        // 百分率を倍率に変換
        EasingVelocityYPercent /= 100;
        EagingSpeedPercent /= 100;
        grideAddSpeed = player.BaseAddSpeed * EagingSpeedPercent;
        decaySpeed = player.BaseAddSpeed * (1 - EagingSpeedPercent);
        // 読み込むファイルのファイル名
        string fileName = nameof(PlayerGlide) + "Data" + player.Type;

       
        // decaySpeed = TextManager.Instance.GetValue_float(fileName, nameof(decaySpeed));        
        SEVolume = TextManager.Instance.GetValue_float(fileName, nameof(SEVolume));

    }

    /// <summary>
    /// テキストからパラメータを取得
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
        // テキストの中のデータをセットするディクショナリー        
        SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
        maxSpeed  = textDataDic["滑空中の最高速度"];
        EagingSpeedPercent = textDataDic["滑空中の加速度が走り状態の何パーセントの加速度になるか(%)"];
        EasingVelocityYPercent = textDataDic["滑空中の落下速度が通常の何パーセントになるか(%)"];

    }

    /// <summary>
    /// 滑空の開始処理
    /// </summary>
    public void StartGlide()
    {
        // SEの再生
        AudioManager.Instance.PlaySE(openSE, SEVolume);
    }

    /// <summary>
    /// 滑空中の処理
    /// </summary>
    public void Gride()
    {
        // x方向への速度変化
        ChangeVelocityX();
        // y方向への速度緩和
        EasingVelocityY();
    }

    /// <summary>
    /// 落下を和らげる処理
    /// </summary>
    void EasingVelocityY()
    {
        var velocity = rigidbody2d.velocity;
        // 落下速度軽減処理
        Vector2 workVec = new Vector2(velocity.x, velocity.y * EasingVelocityYPercent);
        rigidbody2d.velocity = workVec;
    }

    /// <summary>
    /// 速度調整
    /// </summary>
    void ChangeVelocityX()
    {

        // 速度が最高速度以下なら加速
        if (rigidbody2d.velocity.x < maxSpeed)
        {
            SpeedUpToMaxSpeed();
        }
        // 最高速度以上なら減速
        else if (rigidbody2d.velocity.x >= maxSpeed)
        {
            SpeedDownToMaxSpeed();
        }
        // ちょうどなら何もしない
    }

    /// <summary>
    /// 加速処理
    /// </summary>
    void SpeedUpToMaxSpeed()
    {
        // 加速処理
        rigidbody2d.AddForce(new Vector2(grideAddSpeed, 0), ForceMode2D.Force);
        var velocity = rigidbody2d.velocity;
        // 速度が基本速度を下回っていたら基本速度に戻す
        if (player.BaseSpeed > velocity.x)
        {
            velocity.x = player.BaseSpeed;
        }
        // 速度が最高速度を上回っていたら最高速度に戻す
        if (maxSpeed < velocity.x)
        {
            velocity.x = maxSpeed;
        }

        rigidbody2d.velocity = velocity;
    }

    /// <summary>
    /// 減速処理
    /// </summary>
    void SpeedDownToMaxSpeed()
    {
        //// 減速前の速度
        //var beforeVelocity = rigidbody2d.velocity;
        //// 減速後の速度
        Vector2 afterVelocity;
        //// 減速処理
        rigidbody2d.AddForce(new Vector2(-decaySpeed, 0), ForceMode2D.Force);
        afterVelocity = rigidbody2d.velocity;
        // 減速後の速度が最高速度を下回っていたら最高速度に戻す
        if (afterVelocity.x < maxSpeed)
        {
            rigidbody2d.velocity = new Vector2(maxSpeed, afterVelocity.y);
        }
        // そうでなければ減速後処理を適応
        else
        {
            rigidbody2d.velocity = afterVelocity;
        }
    }

    /// <summary>
    /// 滑空の終了処理
    /// </summary>
    public void EndGlide()
    {
        if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        }
    }
}
